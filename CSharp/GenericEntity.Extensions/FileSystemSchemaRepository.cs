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

        public FileSystemSchemaRepository()
        {
        }

        public FileSystemSchemaRepository(string baseDirectory)
        {
            this.baseDirectory = baseDirectory;
        }

        /// <inheritdoc/>
        public SchemaInfo GetSchema(string id)
        {
            string schemaFileName = string.IsNullOrEmpty(this.baseDirectory) ? id : Path.Combine(this.baseDirectory, id);
            FileInfo fileInfo = new FileInfo(schemaFileName);

            return new SchemaInfo() { Id = id, Format = fileInfo.Extension.TrimStart(new char[] { '.' }).ToLower(), RawSchema = File.ReadAllText(schemaFileName), Uri = new UriBuilder("file", "localhost") { Path = fileInfo.FullName }.Uri };
        }
    }
}
