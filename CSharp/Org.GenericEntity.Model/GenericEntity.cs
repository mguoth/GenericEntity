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
using System.Net;
using System.Text.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Linq.Expressions;
using System.Xml.Linq;

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
            this.SchemaInfo = schemaInfo;

            GenericSchema schema = GetSchema(schemaInfo);
            Fields = BuildFields(schema, false);
        }

        internal GenericEntity(GenericEntityDto dto, SchemaInfo schemaInfo)
        {
            this.SchemaInfo = schemaInfo;
 
            GenericSchema schema = GetSchema(schemaInfo);
            
            //When deserializing treat field names as case insensitive by default
            this.Fields = BuildFields(schema, true);

            foreach (var keyValuePair in dto.Fields)
            {
                if (Fields.TryGetField(keyValuePair.Key, out IField field))
                {
                    if (keyValuePair.Value is IFieldValueProvider jsonValueProvider)
                    {
                        //Set value provider
                        ImportFieldValue(field, jsonValueProvider);
                    }
                    else
                    {
                        //Set scalar value
                        field.SetValue(keyValuePair.Value);
                    }
                }
            }

            //After deserializing, treat field names as sensitive
            this.Fields = this.Fields.AsCaseSensitive();
        }

        /// <summary>
        /// Gets a value indicating whether field name is case insensitive.
        /// </summary>
        public bool FieldNameCaseInsensitive { get; }

        private GenericSchema GetSchema(SchemaInfo schemaInfo)
        {
            ISchemaParser schemaParser = ((GenericEntityExtensions)Extensions).GetSchemaParser(schemaInfo.Format);

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

            return schema;
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

        /// <summary>
        /// Creates the <see cref="GenericEntity" /> instance from specified object
        /// </summary>
        /// <param name="schemaInfo">The schema information.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static GenericEntity FromObject(SchemaInfo schemaInfo, object obj, ConverterOptions options = null)
        {
            using (Stream stream = new MemoryStream())
            {
                JsonSerializer.Serialize(stream, obj);

                JsonObject document = (JsonObject) JsonSerializer.SerializeToNode(obj);
                document["$genericEntity"] = JsonSerializer.SerializeToNode(new GenericEntityInfo() { SchemaUri = schemaInfo.Uri, SchemaFormat = schemaInfo.Format });

                //reset stream position
                GenericEntity genericEntity = JsonSerializer.Deserialize<GenericEntity>(document, (options == null) ? null : new JsonSerializerOptions() { PropertyNameCaseInsensitive = options.FieldNameCaseInsensitive });
                return genericEntity;
            }
        }

        /// <summary>
        /// Converts into specified object type
        /// </summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public T ToObject<T>(ConverterOptions options = null)
        {
            using (Stream stream = new MemoryStream())
            {
                JsonSerializer.Serialize(stream, this);

                //reset stream position
                stream.Position = 0;
                T obj = JsonSerializer.Deserialize<T>(stream, (options == null) ? null : new JsonSerializerOptions() { PropertyNameCaseInsensitive = options.FieldNameCaseInsensitive });
                return obj;
            }
        }

        private FieldCollection BuildFields(GenericSchema schema, bool caseInsensitive)
        {
            FieldCollectionBuilder builder = new FieldCollectionBuilder();

            foreach (FieldDefinition fieldDefinition in schema.Fields)
            {
                IField field = CreateField(fieldDefinition);
                builder.Add(field);
            }

            return builder.Build(caseInsensitive);
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
