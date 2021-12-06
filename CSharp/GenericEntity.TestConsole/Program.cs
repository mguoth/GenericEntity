using GenericEntity.Abstractions;
using GenericEntity.Extensions;
using System;
using System.IO;
using System.Text.Json;
using GenericEntity;
using System.Linq;
using System.Reflection;

namespace GenericEntity.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine($"Syntax: {Assembly.GetExecutingAssembly().GetName().Name}.exe $schema");
                return;
            }

            string schema = args[0];

            //Initialization of the schema repository
            ISchemaRepository schemaRepository = new JsonFileSchemaRepository("Schemas");
            GenericEntity.DefaultSchemaRepository = schemaRepository;

            //Creating address entity
            GenericEntity address = new GenericEntity(schema, schemaRepository);

            ReadFromConsole(address);

            PrintToConsole(address);
        }

        private static void ReadFromConsole(GenericEntity genericEntity)
        {
            Console.WriteLine($@"Please type generic entity ""{genericEntity.Schema.EntityType}"" fields values:");

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
