using GenericEntity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericEntity.Tests
{
    public class FileSystemSchemaRepositoryTests
    {
        [Theory]
        [InlineData(@"Schemas", "Address.avsc", "file://localhost/{currentDirectory}/Schemas/Address.avsc")]
        [InlineData(@"{currentDirectory}\Schemas", "Address.avsc", "file://localhost/{currentDirectory}/Schemas/Address.avsc")]
        [InlineData(@"\\localhost\Schemas", "Address.avsc", "file://localhost/Schemas/Address.avsc")]
        [InlineData(@"\\SKBRA-8CRS2J3\Schemas", "Address.avsc", "file://SKBRA-8CRS2J3/Schemas/Address.avsc")]
        [InlineData(@"\\unknownserver\Schemas", "Address.avsc", "file://unknownserver/Schemas/Address.avsc")]
        public void GetSchemaUri(string basePath, string schemaId, string expectedUri)
        {
            basePath = ProcessPlaceHolders(basePath);
            schemaId = ProcessPlaceHolders(schemaId);
            expectedUri = ProcessPlaceHolders(expectedUri);

            FileSystemSchemaRepository schemaRepository = new FileSystemSchemaRepository(basePath);

            string schemaFilePath = schemaRepository.GetSchemaFilePath(schemaId);
            Uri schemaUri = schemaRepository.FilePathToUri(schemaFilePath);

            Assert.Equal(new Uri(expectedUri), schemaUri);
        }

        private string ProcessPlaceHolders(string input)
        {
            return input.Replace("{currentDirectory}", Directory.GetCurrentDirectory());
        }
    }
}
