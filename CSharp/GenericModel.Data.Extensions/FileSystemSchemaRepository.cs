using GenericModel.Data.Abstractions;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Web;

namespace GenericModel.Data.Extensions
{
    [SchemaRepository("file")]
    public class FileSystemSchemaRepository : ISchemaRepository
    {
        private readonly string basePath;

        public FileSystemSchemaRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemSchemaRepository"/> class.
        /// </summary>
        /// <param name="basePath">The base path supporting UNC.</param>
        public FileSystemSchemaRepository(string basePath)
        {
            this.basePath = basePath;
        }

        /// <summary>
        /// Converts file path to URI.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public Uri FilePathToUri(string filePath)
        {
            Uri uri = null;
            try
            {
                uri = new Uri(filePath);
            }
            catch (Exception)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                filePath = fileInfo.FullName;
                uri = new Uri(filePath);
            }

            //get rid of original string as it is used in serialisers and escape it
            uri = new Uri(uri.AbsoluteUri);

            return uri;
        }

        /// <summary>
        /// Converts URI to file path.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public string UriToFilePath(Uri uri)
        {
            if (!uri.IsFile)
            {
                throw new NotSupportedException($@"The ""{uri}"" Uri is not a valid file schema");
            }

            return uri.LocalPath;
        }

        /// <inheritdoc/>
        public SchemaInfo GetSchema(string id)
        {
            string filePath = GetSchemaFilePath(id);
            FileInfo fileInfo = new FileInfo(filePath);

            string rawSchema = File.ReadAllText(filePath);

            SchemaInfo schemaInfo = new SchemaInfo()
            {
                Id = id,
                Format = fileInfo.Extension.TrimStart(new char[] { '.' }).ToLower(),
                RawSchema = rawSchema,
                Uri = FilePathToUri(filePath)
            };

            return schemaInfo;
        }

        /// <inheritdoc/>
        public SchemaInfo GetSchema(Uri uri)
        {
            string filePath = UriToFilePath(uri);

            FileInfo fileInfo = new FileInfo(filePath);

            string rawSchema = File.ReadAllText(filePath);
            
            SchemaInfo schemaInfo = new SchemaInfo()
            {
                Id = uri.Segments.Last(),
                Format = fileInfo.Extension.TrimStart(new char[] { '.' }).ToLower(),
                RawSchema = rawSchema,
                Uri = uri
            };

            return schemaInfo;
        }

        /// <summary>
        /// Gets the schema file path.
        /// </summary>
        /// <param name="schemaId">The schema identifier.</param>
        /// <returns></returns>
        public string GetSchemaFilePath(string schemaId)
        {
            return string.IsNullOrEmpty(this.basePath) ? schemaId : Path.Combine(this.basePath, schemaId);
        }
    }
}
