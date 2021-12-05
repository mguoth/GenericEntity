using GenericEntity.Abstractions;
using GenericEntity.Extensions;
using System;
using System.IO;
using System.Text.Json;
using GenericEntity;
using System.Linq;

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

            //Enumerating fields and setting the string value (which is validated and converted into proper field type)
            foreach (IField field in genericEntity.Fields)
            {
                while (true)
                {
                    Console.Write($"{field.Definition.DisplayName} ({field.Definition.Type}): ");
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
            int[] columnWidths = new int[] { 15, 20, 15, 15 };
            Console.WriteLine($@"Printing generic entity ""{genericEntity.Schema.EntityType}"" fields:");

            int i = 0;
            Console.WriteLine($"{"Name".PadRight(columnWidths[i++])}{"DisplayName".PadRight(columnWidths[i++])}{"Type".PadRight(columnWidths[i++])}{"NET Type".PadRight(columnWidths[i++])}{"Value"}");
            Console.WriteLine($"{"-".PadRight(columnWidths.Sum() + 5, '-')}");

            //Enumerating fields and getting value as string (such conversion is safe for all field types)
            foreach (IField field in genericEntity.Fields)
            {
                i = 0;
                Console.WriteLine($"{field.Definition.Name.PadRight(columnWidths[i++])}{field.Definition.DisplayName.PadRight(columnWidths[i++])}{field.Definition.Type.PadRight(columnWidths[i++])}{field.DataType.ToString().PadRight(columnWidths[i++])}{field.GetString()}");
            }
            Console.WriteLine();
        }
    }
}
