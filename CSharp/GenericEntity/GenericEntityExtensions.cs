using GenericEntity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GenericEntity
{
    public class GenericEntityExtensions : IGenericEntityExtensions
    {
        private readonly IDictionary<string, Type> schemaParserRegistry = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, Type> schemaRepositoryRegistry = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, ISchemaParser> schemaParserInstances = new Dictionary<string, ISchemaParser>(StringComparer.OrdinalIgnoreCase);
        private readonly IDictionary<string, ISchemaRepository> schemaRepositoryInstances = new Dictionary<string, ISchemaRepository>(StringComparer.OrdinalIgnoreCase);

        public void RegisterExtension(Assembly assembly)
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
                schemaParser = (ISchemaParser) schemaParserRegistry[format].GetConstructor(new Type[0]).Invoke(new object[0]);
                schemaParserInstances[format] = schemaParser;
            }

            return schemaParser;
        }
    }
}
