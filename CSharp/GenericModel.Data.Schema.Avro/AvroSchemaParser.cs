using Avro;
using Avro.Generic;
using GenericModel.Data.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace GenericModel.Data.Schema.Avro
{
    [SchemaParser("avsc")]
    public class AvroSchemaParser : ISchemaParser
    {
        /// <inheritdoc />
        GenericSchema ISchemaParser.Parse(string rawSchema)
        {
            RecordSchema schema = (RecordSchema) global::Avro.Schema.Parse(rawSchema);

            using (JsonDocument jsonDocument = JsonDocument.Parse(rawSchema))
            {
                List<string> fieldSchemas = jsonDocument.RootElement
                    .GetProperty("fields")
                    .EnumerateArray()
                    .Select(x => x.ToString())
                    .ToList();

                GenericSchema genericSchema = new GenericSchema();
                genericSchema.RawSchema = rawSchema;

                genericSchema.Fields = schema.Fields.Select((x) => new { Field=x, FieldValueInfo = GetFieldValueInfo(x) })
                    .Select((x, index) => new FieldDefinition()
                    {
                        RawSchema = fieldSchemas[index],
                        Name = x.Field.Name,
                        Description = x.Field.Documentation,
                        ValueType = x.FieldValueInfo.valueType,
                        DefaultValue = x.FieldValueInfo.defaultValue,
                        Nullable = x.FieldValueInfo.nullable,
                        DisplayName = x.Field.GetProperty("displayName").Trim('\"')
                    }).ToArray();

                return genericSchema;
            }
        }

        private (Type valueType, bool nullable, object? defaultValue) GetFieldValueInfo(global::Avro.Field field)
        {
            string fieldType;
            bool nullable = false;
            if (field.Schema is UnionSchema unionSchema)
            {
                if (unionSchema.Schemas.Count != 2
                    || unionSchema.Schemas.Where(x => x.Name.Equals("null", StringComparison.InvariantCultureIgnoreCase)).Count() != 1)
                {
                    throw new NotSupportedException($@"The field type union support is restricted to combination of ""null"" and other primitive type");
                }

                nullable = true;
                fieldType = unionSchema.Schemas.Where(x => !x.Name.Equals("null", StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault().Name;
            }
            else
            {
                fieldType = field.Schema.Name;
            }

            Type valueType = default!;
            object? defaultValue = null;
            switch (fieldType.ToLower())
            {
                case "boolean":
                    valueType = nullable ? typeof(bool?) : typeof(bool);
                    defaultValue = (field.DefaultValue == null || field.DefaultValue.Type == Newtonsoft.Json.Linq.JTokenType.Null) ? (nullable ? null : (object) default(bool)) : field.DefaultValue.ToObject<bool>();
                    break;
                case "int":
                    valueType = nullable ? typeof(int?) : typeof(int);
                    defaultValue = (field.DefaultValue == null || field.DefaultValue.Type == Newtonsoft.Json.Linq.JTokenType.Null) ? (nullable ? null : (object) default(int)) : field.DefaultValue.ToObject<int>();
                    break;
                case "long":
                    valueType = nullable ? typeof(long?) : typeof(long);
                    defaultValue = (field.DefaultValue == null || field.DefaultValue.Type == Newtonsoft.Json.Linq.JTokenType.Null) ? (nullable ? null : (object) default(long)) : field.DefaultValue.ToObject<long>();
                    break;
                case "float":
                    valueType = nullable ? typeof(float?) : typeof(float);
                    defaultValue = (field.DefaultValue == null || field.DefaultValue.Type == Newtonsoft.Json.Linq.JTokenType.Null) ? (nullable ? null : (object) default(float)) : field.DefaultValue.ToObject<float>();
                    break;
                case "double":
                    valueType = nullable ? typeof(double?) : typeof(double);
                    defaultValue = (field.DefaultValue == null || field.DefaultValue.Type == Newtonsoft.Json.Linq.JTokenType.Null) ? (nullable ? null : (object) default(double)) : field.DefaultValue.ToObject<double>();
                    break;
                case "bytes":
                    valueType = typeof(byte[]);
                    if (field.DefaultValue != null && field.DefaultValue.Type != Newtonsoft.Json.Linq.JTokenType.Null)
                    {
                        throw new NotSupportedException($@"The field type ""bytes"" default value is not yet supported.");
                    }
                    break;
                case "string":
                    valueType = typeof(string);
                    defaultValue = (field.DefaultValue == null || field.DefaultValue.Type == Newtonsoft.Json.Linq.JTokenType.Null) ? (nullable ? null : String.Empty) : (object?) field.DefaultValue?.ToObject<string>();
                    break;
                default:
                    throw new NotSupportedException($@"The field type ""{field.Schema.Name}"" is not yet supported.");
            }

            return (valueType, nullable, defaultValue);
        }
    }
}
