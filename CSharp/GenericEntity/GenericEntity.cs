using GenericEntity.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace GenericEntity
{
    /// <summary>
    /// Generic entity
    /// </summary>
    public partial class GenericEntity
    {
        private static readonly object syncRoot = new object();
        private static readonly IDictionary<string, Schema> compiledSchemaCache = new Dictionary<string, Schema>();

        public GenericEntity(string schemaName, ISchemaRepository schemaRepository)
        {
            this.SchemaName = schemaName;

            SchemaCompiler schemaCompiler = new SchemaCompiler(schemaRepository, Assembly.Load("GenericEntity.Extensions"));

            Schema schema;
            lock (syncRoot)
            {
                if (compiledSchemaCache.TryGetValue(schemaName, out schema))
                {
                    //Get from cache if exists
                    this.Schema = schema;
                }
                else
                {
                    //Compile and index into cache
                    this.Schema = schemaCompiler.Compile(schemaName);
                    compiledSchemaCache[schemaName] = this.Schema;
                }
            }

            this.Fields = BuildFields();
        }

        public GenericEntity(GenericEntityDto dto, ISchemaRepository schemaRepository) : this(dto.SchemaName, schemaRepository)
        {
            foreach (var field in dto.Fields)
            {
                if (field.Value is IJsonValueProvider jsonValueProvider)
                {
                    //Set value provider
                    this.ImportFieldValue(this.Fields[field.Key], jsonValueProvider);
                }
                else
                {
                    //Set scalar value
                    this.Fields[field.Key].Set(field.Value);
                }
            }
        }

        public string SchemaName { get; }

        public Schema Schema { get; }

        /// <summary>
        /// Gets fields
        /// </summary>
        public FieldCollection Fields { get; }

        /// <summary>
        /// Converts to Dto.
        /// </summary>
        public GenericEntityDto ToDto()
        {
            GenericEntityDto dto = new GenericEntityDto();
            dto.SchemaName = this.SchemaName;

            foreach (IField field in this.Fields)
            {
                dto.Fields.Add(field.Definition.Name, field.Get<object>());
            }
            return dto;
        }

        private FieldCollection BuildFields()
        {
            FieldCollectionBuilder builder = new FieldCollectionBuilder();
          
            foreach (FieldDefinition fieldDefinition in this.Schema.Fields)
            {
                IField field = this.CreateField(fieldDefinition);
                builder.Add(field);
            }

            return builder.Build();
        }
        
        private IField CreateField(FieldDefinition fieldDefinition)
        {
            IField field = (IField)fieldDefinition.FieldType.GetConstructor(new Type[] { typeof(IFieldDefinition) }).Invoke(new object[] { fieldDefinition });
            return field;
        }

        private void ImportFieldValue(IField field, IJsonValueProvider jsonValueProvider)
        {
            switch (field.Definition.Type)
            {
                case "string":
                    field.Set(jsonValueProvider.GetString());
                    break;

                case "integer":
                    field.Set(jsonValueProvider.GetInteger());
                    break;
            }
        }
    }
}
