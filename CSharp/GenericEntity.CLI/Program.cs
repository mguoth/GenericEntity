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
            address.Fields["id"].SetValue(1);
            address.Fields["addressLine1"].SetValue("Wall Street 35");
            address.Fields["city"].SetValue("New York");
            address.Fields["postalCode"].SetValue("10030");
            address.Fields["country"].SetValue("US");

            //Create DTO and serialise it
            GenericEntityDto addressDto = address.ToDto();
            string json = JsonSerializer.Serialize(addressDto, new JsonSerializerOptions() { WriteIndented = true });
            
            //Deserialise DTO and reconstruct entity            
            GenericEntityDto reconstructedAddressDto = JsonSerializer.Deserialize<GenericEntityDto>(json);
            GenericEntity reconstructedAddress = new GenericEntity(reconstructedAddressDto, schemaRepository);

            //Enumerating fields and getting value as string (to string conversion is supported by all field types)
            Console.WriteLine($"({reconstructedAddress.Schema.EntityType})");
            foreach (IField field in reconstructedAddress.Fields)
            {
                Console.WriteLine($"|- {field.Definition.Name} ({field.DataType}): {field.GetValue<string>()}");
            }
        }
    }
}
