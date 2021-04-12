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
            address.Fields["id"].AsInt32().Value = 1;
            address.Fields["addressLine1"].AsString().Value = "Wall Street 35";
            address.Fields["city"].AsString().Value = "New York";
            address.Fields["postalCode"].AsString().Value = "10030";
            address.Fields["country"].AsString().Value = "US";

            //Create DTO and serialise it
            GenericEntityDto addressDto = address.ToDto();
            string json = JsonSerializer.Serialize(addressDto);
            
            //Deserialise DTO and reconstruct entity            
            GenericEntityDto reconstructedAddressDto = JsonSerializer.Deserialize<GenericEntityDto>(json);
            GenericEntity reconstructedAddress = new GenericEntity(reconstructedAddressDto, schemaRepository);

            //Enumerating fields and getting value as string (to string conversion is supported by all field types)
            Console.WriteLine($"({reconstructedAddress.Schema.EntityType})");
            foreach (IField field in reconstructedAddress.Fields)
            {
                Console.WriteLine($"|- {field.Definition.Name} ({field.DataType}): {field.AsString().Value}");
            }
        }
    }
}
