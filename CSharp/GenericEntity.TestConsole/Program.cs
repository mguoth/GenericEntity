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

            ReadFromConsole(address);

            PrintToConsole(address);

            Console.WriteLine($"Serialising it into JSON." + Environment.NewLine);

            //Serialise generic entity into Json
            string json = JsonSerializer.Serialize(address, new JsonSerializerOptions() { WriteIndented = true });
            
            Console.WriteLine(json + Environment.NewLine);

            Console.WriteLine($"Deserialising it back into generic entity." + Environment.NewLine);

            //Deserialise generic entity from Json
            GenericEntity reconstructedAddress = JsonSerializer.Deserialize<GenericEntity>(json);

            PrintToConsole(reconstructedAddress);
        }

        private static void ReadFromConsole(GenericEntity genericEntity)
        {
            Console.WriteLine($@"Reading generic entity ""{genericEntity.Schema.EntityType}"" fields:");

            foreach (IField field in genericEntity.Fields)
            {
                while (true)
                {
                    Console.Write($"{field.Definition.Name} ({field.Definition.Type}): ");
                    string value = Console.ReadLine();

                    try
                    {
                        field.SetString(value);
                        break;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(@$"Can't set ""{value}"" into ""{field.Definition.Type}"" field type.");
                    }
                }
            }
            Console.WriteLine("Reading completed." + Environment.NewLine);
        }

        private static void PrintToConsole(GenericEntity genericEntity)
        {
            //Enumerating fields and getting value as string (to string conversion is supported by all field types)
            Console.WriteLine($@"Printing generic entity ""{genericEntity.Schema.EntityType}"" fields:");
            
            Console.WriteLine($"{"Name".PadRight(20)}{"Type".PadRight(15)}{"NET Type".PadRight(15)}{"Value"}");
            Console.WriteLine($"{"-".PadRight(60, '-')}");

            foreach (IField field in genericEntity.Fields)
            {
                int fieldNameTabs = field.Definition.Name.Length / 8;

                Console.WriteLine($"{field.Definition.Name.PadRight(20)}{field.Definition.Type.PadRight(15)}{field.DataType.ToString().PadRight(15)}{field.GetString()}");
            }
            Console.WriteLine();
        }
    }
}
