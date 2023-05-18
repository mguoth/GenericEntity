using GenericModel.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenericModel.Data
{
    public class GenericEntityExtensions : IGenericEntityExtensions
    {
        private readonly IDictionary<string, Type> schemaParserRegistry = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, Type> schemaRepositoryRegistry = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, ISchemaParser> schemaParserInstances = new Dictionary<string, ISchemaParser>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, ISchemaRepository> schemaRepositoryInstances = new Dictionary<string, ISchemaRepository>(StringComparer.OrdinalIgnoreCase);

        public void AddExtension(Assembly assembly)
        {
            foreach (var item in assembly.ScanTypesForCustomAttributes<SchemaParserAttribute>())
            {
                schemaParserRegistry.Add(item.Attribute.Format, item.Type);
            }

            foreach (var item in assembly.ScanTypesForCustomAttributes<SchemaRepositoryAttribute>())
            {
                schemaRepositoryRegistry.Add(item.Attribute.Name, item.Type);
            }
        }

        public ISchemaRepository GetSchemaRepository(string name)
        {
            ISchemaRepository schemaRepository;
            if (!schemaRepositoryInstances.TryGetValue(name, out schemaRepository))
            {
                schemaRepository = (ISchemaRepository) schemaRepositoryRegistry[name].GetConstructor(new Type[0]).Invoke(new object[0]);
                schemaRepositoryInstances[name] = schemaRepository;
            }

            return schemaRepository;
        }

        public ISchemaParser GetSchemaParser(string format)
        {
            ISchemaParser schemaParser;
            if (!schemaParserInstances.TryGetValue(format, out schemaParser))
            {
                if (schemaParserRegistry.TryGetValue(format, out Type schemaParserType))
                {
                    schemaParser = (ISchemaParser) schemaParserType.GetConstructor(new Type[0]).Invoke(new object[0]);
                    schemaParserInstances[format] = schemaParser;
                }
                else
                {
                    throw new NotSupportedException($@"Not supported schema format ""{format}""");
                }
            }

            return schemaParser;
        }
    }
}
