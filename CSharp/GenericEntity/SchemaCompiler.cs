using GenericEntity.Abstractions;
using SchemaModel = Schema.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace GenericEntity
{
    public class SchemaCompiler
    {
        private readonly ISchemaRepository schemaRepository;
        private readonly IEnumerable<Assembly> extensionsAssemblies;
        
        public SchemaCompiler(ISchemaRepository schemaRepository, Assembly extensionAssembly)
        {
            if (extensionAssembly == null)
            {
                throw new ArgumentException($@"Not provided extensions assemblies argument");
            }

            this.schemaRepository = schemaRepository;
            this.extensionsAssemblies = new Assembly[] { extensionAssembly };
        }

        public Schema Compile(string schemaName)
        {
            SchemaModel.SchemaDefinition schemaDefinition = this.schemaRepository.GetSchema(schemaName);

            var scannedTypes = extensionsAssemblies.ScanTypesForCustomAttributes<FieldTypeAttribute>();

            //Validate scanned types from assembly
            foreach (var attributeInstance in scannedTypes)
            {
                if (!typeof(IField).IsAssignableFrom(attributeInstance.Type))
                {
                    throw new InvalidOperationException($@"The ""{attributeInstance.Type.FullName}"" doesn't implement ""{typeof(IField)}"" interface");
                }
            }

            //Remove field types not used in this schema and build dictionary
            IDictionary<string, Type> fieldTypes = scannedTypes.Where(x => schemaDefinition.Fields.Select(y => y.Type).Contains(x.Attribute.FieldDefinitionType))
                                                               .ToDictionary(x => x.Attribute.FieldDefinitionType, y => y.Type);

            foreach (SchemaModel.FieldDefinition fieldDefinition in schemaDefinition.Fields)
            {
                Type fieldType;
                if (!fieldTypes.TryGetValue(fieldDefinition.Type, out fieldType))
                {
                    throw new InvalidOperationException($@"The field type ""{fieldDefinition.Type}"" was not found in registered extensions assemblies");
                }
            }

            SchemaCompilerData compilerData = new SchemaCompilerData(fieldTypes);

            return new Schema(schemaDefinition, compilerData);
        }
    }
}
