using GenericEntity.Abstractions;
using GenericEntity.Avro;
using GenericEntity.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;

namespace GenericEntity.Tests
{
    public class FunctionalTests
    {
        [Fact]
        public void JsonSerialisation()
        {
            //Initialization of the schema repository
            ISchemaRepository schemaRepository = new InMemorySchemaRepository()
            {
                {
                    "address",
                    "avro",
                    @$"{{
                        ""type"": ""record"",
                        ""name"": ""Address"",
                        ""namespace"": ""GenericEntity.Samples"",
                        ""fields"": [
                        {{
                            ""name"": ""id"",
                            ""type"": [""null"", ""int""],
                            ""displayName"": ""Id"",
                            ""doc"": ""The unique identifier""
                        }},
                        {{
                            ""name"": ""addressLine1"",
                            ""type"": ""string"",
                            ""displayName"": ""Address line 1""
                        }},
                        {{
                            ""name"": ""addressLine2"",
                            ""type"": [""null"", ""string""],
                            ""displayName"": ""Address line 2""
                        }},
                        {{
                            ""name"": ""city"",
                            ""type"": ""string"",
                            ""displayName"": ""City""
                        }},
                        {{
                            ""name"": ""postalCode"",
                            ""type"": ""string"",
                            ""displayName"": ""Postal code""
                        }},
                        {{
                            ""name"": ""country"",
                            ""type"": ""string"",
                            ""displayName"": ""Country""
                        }}
                        ]
                    }}"
                }
            };

            //Adding generic entity extensions
            GenericEntity.Extensions.AddStandard();
            GenericEntity.Extensions.AddAvro();

            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("address");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            address.Fields["id"].SetValue(1);
            address.Fields["addressLine1"].SetValue("2501 Redbud Drive");
            address.Fields["city"].SetValue("New York");
            address.Fields["postalCode"].SetValue("10011");
            address.Fields["country"].SetValue("United States");

            //Serialise
            string addressJson = JsonSerializer.Serialize(address);

            GenericEntity reconstructedAddress = JsonSerializer.Deserialize<GenericEntity>(addressJson);

            string reconstructedAddressJson = JsonSerializer.Serialize(reconstructedAddress);

            Assert.Equal(addressJson, reconstructedAddressJson);
        }
    }
}