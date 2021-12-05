using GenericEntity.Abstractions;
using GenericEntity.Extensions;
using System;
using System.IO;
using System.Text.Json;
using GenericEntity;

namespace GenericEntity.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialization
            ISchemaRepository schemaRepository = new JsonFileSchemaRepository("Schemas");
            GenericEntity.DefaultSchemaRepository = schemaRepository;

            //Creating address entity
            GenericEntity address = new GenericEntity("Address", schemaRepository);

            //Direct access to fields and strongly typed values
            address.Fields["id"].SetInteger(1);
            address.Fields["addressLine1"].SetString("Wall Street 35");
            address.Fields["city"].SetString("New York");
            address.Fields["postalCode"].SetString("10030");
            address.Fields["country"].SetString("US");

            //Serialise generic entity into Json
            string json = JsonSerializer.Serialize(address, new JsonSerializerOptions() { WriteIndented = true });

            //Deserialise generic entity from Json
            GenericEntity reconstructedAddress = JsonSerializer.Deserialize<GenericEntity>(json);

            //Enumerating fields and getting value as string (to string conversion is supported by all field types)
            Console.WriteLine($"{address.Schema.EntityType} {nameof(address)} ({address.GetType()})");
            foreach (IField field in reconstructedAddress.Fields)
            {
                Console.WriteLine($"|- {field.Definition.Type} {field.Definition.Name}: {field.GetString()} ({field.DataType})");
            }
        }
    }
}
