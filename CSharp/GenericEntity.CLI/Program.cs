using GenericEntity.Abstractions;
using GenericEntity.Extensions;
using System;
using System.IO;
using System.Text.Json;

namespace GenericEntity.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ISchemaRepository schemaRepository = new JsonFileSchemaRepository("Schemas");

            //Creating address entity
            GenericEntity address = new GenericEntity("Address", schemaRepository);

            //Direct access to fields and strongly typed values
            address.Fields["id"].SetInteger(1);
            address.Fields["addressLine1"].SetString("Wall Street 35");
            address.Fields["city"].SetString("New York");
            address.Fields["postalCode"].SetString("10030");
            address.Fields["country"].SetString("US");

            //Create DTO and serialise it
            GenericEntityDto addressDto = address.ToDto();
            string json = JsonSerializer.Serialize(addressDto, new JsonSerializerOptions() { WriteIndented = true });
            
            //Deserialise DTO and reconstruct entity            
            GenericEntityDto reconstructedAddressDto = JsonSerializer.Deserialize<GenericEntityDto>(json);
            GenericEntity reconstructedAddress = new GenericEntity(reconstructedAddressDto, schemaRepository);

            //Enumerating fields and getting value as string (to string conversion is supported by all field types)
            Console.WriteLine($"{address.SchemaName} {nameof(address)} ({address.GetType()})");
            foreach (IField field in reconstructedAddress.Fields)
            {
                Console.WriteLine($"|- {field.Definition.Type} {field.Definition.Name}: {field.GetString()} ({field.DataType})");
            }
        }
    }
}
