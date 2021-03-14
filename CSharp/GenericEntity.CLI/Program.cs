using GenericEntity.Abstractions;
using GenericEntity.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GenericEntity.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ISchemaRepository schemaRepository = new JsonFileSchemaRepository("Schemas");

            //Creating address entity
            GenericEntity address = new GenericEntity("Address", schemaRepository);
            address.Fields["id"].SetInt32().Value = 1;
            address.Fields["addressLine1"].SetString().Value = "Wall Street 35";
            address.Fields["city"].SetString().Value = "New York";
            address.Fields["postalCode"].SetString().Value = "10030";
            address.Fields["country"].SetString().Value = "US";

            //Get field value as int
            int id = address.Fields["id"].GetInt32().Value;

            //Listing field values as strings
            Console.WriteLine($"({address.Schema.EntityType})");
            foreach (IField field in address.Fields)
            {
                Console.WriteLine($"|- {field.Definition.Name} ({field.DataType}): {field.GetString().Value}");               
            }
        }
    }
}
