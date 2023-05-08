using GenericEntity.Abstractions;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Web;

namespace GenericEntity.Extensions
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
        /// Gets the schema URI.
        /// </summary>
        /// <param name="id">The schema identifier.</param>
        /// <returns></returns>
        public Uri GetSchemaUri(string id)
        {
            string filePath = BuildFilePath(id);
            FileInfo fileInfo = new FileInfo(filePath);

            Uri uri = null;
            try
            {
                uri = new Uri(filePath);
            }
            catch (Exception)
            {
            }

            //Fix non-Unc localhost Uris
            if (uri == null || !uri.IsUnc)
            {
                uri = new UriBuilder("file", "localhost") { Path = fileInfo.FullName }.Uri;
            }

            //get rid of original string as it is used in serialisers and escape it
            uri = new Uri(uri.AbsoluteUri);

            return uri;
        }

        /// <inheritdoc/>
        public SchemaInfo GetSchema(string id)
        {
            Uri uri = GetSchemaUri(id);
            string filePath = BuildFilePath(id);
            FileInfo fileInfo = new FileInfo(filePath);

            string rawSchema = File.ReadAllText(filePath);

            SchemaInfo schemaInfo = new SchemaInfo()
            {
                Id = id,
                Format = fileInfo.Extension.TrimStart(new char[] { '.' }).ToLower(),
                RawSchema = rawSchema,
                Uri = uri
            };

            return schemaInfo;
        }

        /// <inheritdoc/>
        public SchemaInfo GetSchema(Uri uri)
        {
            if (!uri.IsFile)
            {
                throw new NotSupportedException($@"The ""{uri}"" Uri is not a valid file schema in the Unc format");
            }

            string filePath = HttpUtility.UrlDecode(uri.AbsolutePath.TrimStart('/'));
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

        private string BuildFilePath(string id)
        {
            return string.IsNullOrEmpty(this.basePath) ? id : Path.Combine(this.basePath, id);
        }
    }
}
