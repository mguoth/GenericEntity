using GenericModel.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericModel.Data.Tests
{
    public class FileSystemSchemaRepositoryTests
    {
        [Theory]
        [InlineData(@"Schemas", "Address.avsc", "file:///{currentDirectory}/Schemas/Address.avsc")]
        [InlineData(@".\Schemas", "Address.avsc", "file:///{currentDirectory}/Schemas/Address.avsc")]
        [InlineData(@"{currentDirectory}\Schemas", "Address.avsc", "file:///{currentDirectory}/Schemas/Address.avsc")]
        [InlineData(@"\\localhost\Schemas", "Address.avsc", "file://localhost/Schemas/Address.avsc")]
        [InlineData(@"\\server1\Schemas", "Address.avsc", "file://server1/Schemas/Address.avsc")]
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

        [Theory]
        [InlineData("file:///{currentDirectory}/Schemas/Address.avsc", @"{currentDirectory}\Schemas\Address.avsc")]
        [InlineData("file://localhost/Schemas/Address.avsc", @"\\localhost\Schemas\Address.avsc")]
        [InlineData("file://server1/Schemas/Address.avsc", @"\\server1\Schemas\Address.avsc")]
        public void UriToFilePath_IsValid(string uri, string expectedFilePath)
        {
            uri = ProcessPlaceHolders(uri).Replace('\\', '/');
            expectedFilePath = ProcessPlaceHolders(expectedFilePath);

            FileSystemSchemaRepository schemaRepository = new FileSystemSchemaRepository();
            string filePath = schemaRepository.UriToFilePath(new Uri(uri));

            //FileInfo fileInfo = new FileInfo(filePath);
            //Assert.True(fileInfo.Exists);
            Assert.Equal(expectedFilePath, filePath);
        }

        private string ProcessPlaceHolders(string input)
        {
            return input.Replace("{currentDirectory}", Directory.GetCurrentDirectory());
        }
    }
}
