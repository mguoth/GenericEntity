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

            Console.WriteLine($"Creating generic entity instance.");
            Console.WriteLine();

            Print(address);

            Console.WriteLine($"Serialising it into JSON.");
            Console.WriteLine();

            //Serialise generic entity into Json
            string json = JsonSerializer.Serialize(address, new JsonSerializerOptions() { WriteIndented = true });
            
            Console.WriteLine(json);
            Console.WriteLine();

            Console.WriteLine($"Deserialising it back into generic entity.");
            Console.WriteLine();

            //Deserialise generic entity from Json
            GenericEntity reconstructedAddress = JsonSerializer.Deserialize<GenericEntity>(json);

            Print(reconstructedAddress);
        }

        private static void Print(GenericEntity genericEntity)
        {
            //Enumerating fields and getting value as string (to string conversion is supported by all field types)
            Console.WriteLine($"{genericEntity.Schema.EntityType} entity ({genericEntity.GetType()})");
            foreach (IField field in genericEntity.Fields)
            {
                Console.WriteLine($"|- {field.Definition.Type} {field.Definition.Name}: {field.GetString()} ({field.DataType})");
            }
            Console.WriteLine();
        }
    }
}
