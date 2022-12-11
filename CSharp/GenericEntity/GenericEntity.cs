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
        private static readonly IDictionary<string, CompiledSchema> compiledSchemaCache = new Dictionary<string, CompiledSchema>();
        private CompiledSchema compiledSchema;


        public GenericEntity(string schema, ISchemaRepository schemaRepository)
        {
            SchemaCompiler schemaCompiler = new SchemaCompiler(schemaRepository, Assembly.Load("GenericEntity.Extensions"));

            lock (syncRoot)
            {
                if (compiledSchemaCache.TryGetValue(schema, out compiledSchema))
                {
                    //Get from cache if exists
                }
                else
                {
                    //Compile and index into cache
                    compiledSchema = schemaCompiler.Compile(schema);

                    compiledSchemaCache[schema] = compiledSchema;
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
                    this.Fields[field.Key].SetValue(field.Value);
                }
            }
        }

        public Abstractions.Schema Schema => compiledSchema.Schema;

        /// <summary>
        /// Gets fields
        /// </summary>
        public FieldCollection Fields { get; }

        private FieldCollection BuildFields()
        {
            FieldCollectionBuilder builder = new FieldCollectionBuilder();
          
            foreach (FieldDefinition fieldDefinition in this.Schema.Fields)
            {
                IField field = this.CreateField(fieldDefinition, compiledSchema.CompilerData.FieldTypes[fieldDefinition.Type]);
                builder.Add(field);
            }

            return builder.Build();
        }
        
        private IField CreateField(FieldDefinition fieldDefinition, Type fieldType)
        {
            IField field = (IField) fieldType.GetConstructor(new Type[] { typeof(FieldDefinition) }).Invoke(new object[] { fieldDefinition });
            return field;
        }

        private void ImportFieldValue(IField field, IJsonValueProvider jsonValueProvider)
        {
            switch (field.Definition.Type)
            {
                case "string":
                    field.SetValue(jsonValueProvider.GetString());
                    break;

                case "integer":
                    field.SetValue(jsonValueProvider.GetInteger());
                    break;
            }
        }
    }
}
