using GenericEntity.Abstractions;
using System;
using System.Globalization;
using System.IO;

namespace GenericEntity.Extensions
{
    [SchemaRepository("file")]
    public class FileSystemSchemaRepository : ISchemaRepository
    {
        private readonly string baseDirectory;
        private readonly string schemaExtension;
        private readonly string schemaFormat;

        public FileSystemSchemaRepository()
        {
        }

        public FileSystemSchemaRepository(string baseDirectory, string schemaFormat)
        {
            this.baseDirectory = baseDirectory;
            this.schemaFormat = schemaFormat;
        }

        /// <inheritdoc/>
        public SchemaInfo GetSchema(string id)
        {
            string schemaFileName = string.IsNullOrEmpty(this.baseDirectory) ? id : Path.Combine(this.baseDirectory, id);
            FileInfo fileInfo = new FileInfo(schemaFileName);

            return new SchemaInfo() { Id = id, Format = this.schemaFormat, Payload = File.ReadAllText(schemaFileName), Uri = new UriBuilder("file", "localhost") { Path = fileInfo.FullName }.Uri.ToString() };
        }
    }
}
