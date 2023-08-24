using Org.GenericEntity.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text.Json.Serialization;
using System.IO;
using System.Runtime.CompilerServices;
using Org.GenericEntity.Model;

namespace Org.GenericEntity.Model
{
    /// <summary>
    /// Generic entity
    /// </summary>
    [JsonConverter(typeof(GenericEntityConverter))]
    public partial class GenericEntity
    {
        private static readonly object syncRoot = new object();
        private static readonly IDictionary<string, GenericSchema> compiledSchemaCache = new Dictionary<string, GenericSchema>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEntity"/> class.
        /// </summary>
        /// <param name="schemaInfo">The schema information.</param>
        public GenericEntity(SchemaInfo schemaInfo)
        {
            SchemaInfo = schemaInfo;

            ISchemaParser schemaParser = ((GenericEntityExtensions) Extensions).GetSchemaParser(schemaInfo.Format);

            GenericSchema schema = null;
            lock (syncRoot)
            {
                //Get from cache or create new
                if (!compiledSchemaCache.TryGetValue(SchemaInfo.Uri.ToString(), out schema))
                {
                    //Compile and index into cache
                    schema = schemaParser.Parse(SchemaInfo.RawSchema);
                    compiledSchemaCache[SchemaInfo.Uri.ToString()] = schema;
                }
            }

            Fields = BuildFields(schema);
        }

        internal GenericEntity(GenericEntityDto dto, SchemaInfo schemaInfo) : this(schemaInfo)
        {
            foreach (var field in dto.Fields)
            {
                if (field.Value is IFieldValueProvider jsonValueProvider)
                {
                    //Set value provider
                    ImportFieldValue(Fields[field.Key], jsonValueProvider);
                }
                else
                {
                    //Set scalar value
                    Fields[field.Key].SetValue(field.Value);
                }
            }
        }

        /// <summary>
        /// Gets extensions.
        /// </summary>
        public static IGenericEntityExtensions Extensions { get; } = new GenericEntityExtensions();

        /// <summary>
        /// Gets the schema information.
        /// </summary>
        public SchemaInfo SchemaInfo { get; }

        /// <summary>
        /// Gets fields
        /// </summary>
        public FieldCollection Fields { get; }

        private FieldCollection BuildFields(GenericSchema schema)
        {
            FieldCollectionBuilder builder = new FieldCollectionBuilder();

            foreach (FieldDefinition fieldDefinition in schema.Fields)
            {
                IField field = CreateField(fieldDefinition);
                builder.Add(field);
            }

            return builder.Build();
        }

        private IField CreateField(FieldDefinition fieldDefinition)
        {
            Type targetFieldType = typeof(Field<>).MakeGenericType(new Type[] { fieldDefinition.ValueType });
            IField field = (IField)targetFieldType.GetConstructor(new Type[] { typeof(FieldDefinition) }).Invoke(new object[] { fieldDefinition });
            return field;
        }

        private void ImportFieldValue(IField field, IFieldValueProvider fieldValueProvider)
        {
            if (field.ValueType == typeof(string))
            {
                field.SetValue(fieldValueProvider.GetString());
            }
            else if (field.ValueType == typeof(int))
            {
                field.SetValue(fieldValueProvider.GetInt32());
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
