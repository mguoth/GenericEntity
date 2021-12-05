using GenericEntity.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.Json.Serialization;

namespace GenericEntity
{
    /// <summary>
    /// Generic entity
    /// </summary>
    [JsonConverter(typeof(GenericEntityConverter))]
    public partial class GenericEntity
    {
        public static ISchemaRepository DefaultSchemaRepository { get; set; }
        private static readonly object syncRoot = new object();
        private static readonly IDictionary<string, Schema> compiledSchemaCache = new Dictionary<string, Schema>();

        public GenericEntity(string schema, ISchemaRepository schemaRepository)
        {
            SchemaCompiler schemaCompiler = new SchemaCompiler(schemaRepository, Assembly.Load("GenericEntity.Extensions"));

            Schema compiledSchema;
            lock (syncRoot)
            {
                if (compiledSchemaCache.TryGetValue(schema, out compiledSchema))
                {
                    //Get from cache if exists
                    this.Schema = compiledSchema;
                }
                else
                {
                    //Compile and index into cache
                    this.Schema = schemaCompiler.Compile(schema);
                    compiledSchemaCache[schema] = this.Schema;
                }
            }

            this.Fields = BuildFields();
        }

        internal GenericEntity(GenericEntityDto dto, ISchemaRepository schemaRepository) : this(dto.Schema, schemaRepository)
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

        public Schema Schema { get; }

        /// <summary>
        /// Gets fields
        /// </summary>
        public FieldCollection Fields { get; }

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
