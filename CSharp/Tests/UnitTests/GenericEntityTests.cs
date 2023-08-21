using Org.GenericEntity.Abstractions;
using Org.GenericEntity.Model;
using Org.GenericEntity.Extensions.FileSystem;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace UnitTests
{
    public class GenericEntityTests
    {
        //Initialization of the schema repository
        private static readonly ISchemaRepository schemaRepository = new FileSystemSchemaRepository("Schemas");

        static GenericEntityTests()
        {
            //Adding generic entity extensions
            GenericEntity.Extensions.AddFileSystemSchemaRepository();
            GenericEntity.Extensions.AddAvroSchema();
        }

        [Fact]
        public void GetField_Exists_ReturnField()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            IField field = address.Fields["id"];
            Assert.NotNull(field);
            Assert.Equal("id", field.Name, ignoreCase: true);
        }

        [Fact]
        public void GetField_NotExists_ThrowException()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            Assert.Throws<KeyNotFoundException>(() => address.Fields["nonExistingField"]);
        }

        [Fact]
        public void TryGetField_Exists_ReturnField()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            bool result = address.Fields.TryGetField("id", out IField field);
            Assert.True(result);
            Assert.NotNull(field);
            Assert.Equal("id", field.Name, ignoreCase: true);
        }

        [Fact]
        public void TryGetField_NotExists_ReturnNull()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            bool result = address.Fields.TryGetField("nonExistingField", out IField field);
            Assert.False(result);
            Assert.Null(field);
        }

        [Fact]
        public void SetFieldValue_CompatibleType_Success()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            Assert.True(address.Fields.TryGetField("id", out IField field));
            Assert.NotNull(field);

            field.SetValue<long>(5);

            Assert.Equal(5, field.GetValue<int>());
        }

        [Fact]
        public void GetFieldValue_CompatibleType_Success()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            Assert.True(address.Fields.TryGetField("id", out IField field));
            Assert.NotNull(field);

            field.SetValue<int>(5);

            Assert.Equal(5, field.GetValue<long>()); 
        }

        [Fact]
        public void SetFieldValue_IncompatibleType_ThrowException()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            Assert.True(address.Fields.TryGetField("id", out IField field));
            Assert.NotNull(field);

            Assert.Throws<InvalidOperationException>(() => field.SetValue("invalid number"));
        }

        [Fact]
        public void GetFieldValue_IncompatibleType_ThrowException()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            Assert.True(address.Fields.TryGetField("id", out IField field));
            Assert.NotNull(field);

            field.SetValue(5);

            Assert.Throws<InvalidOperationException>(() => field.GetValue<DateTime>());
        }

        [Fact]
        public void TrySetFieldValue_IncompatibleType_ReturnFalse()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            Assert.True(address.Fields.TryGetField("id", out IField field));
            Assert.NotNull(field);

            Assert.False(field.TrySetValue("invalid number"));
        }

        [Fact]
        public void TryGetFieldValue_IncompatibleType_ReturnFalse()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            bool result = address.Fields.TryGetField("id", out IField field);
            Assert.True(result);
            Assert.NotNull(field);

            field.SetValue(5);

            Assert.False(field.TryGetValue(out DateTime value));
        }

        [Fact]
        public void JsonSerialisation()
        {
            //Creating address entity
            SchemaInfo schemaInfo = schemaRepository.GetSchema("Address.avsc");
            GenericEntity address = new GenericEntity(schemaInfo, GenericEntity.Extensions.GetSchemaParser(schemaInfo.Format));

            address.Fields["id"].SetValue(1);
            address.Fields["addressLine1"].SetValue("2501 Redbud Drive");
            address.Fields["city"].SetValue("New York");
            address.Fields["postalCode"].SetValue("10011");
            address.Fields["country"].SetValue("United States");

            //Serialise
            string json = JsonSerializer.Serialize(address);

            //Deserialise
            GenericEntity? reconstructedAddress = JsonSerializer.Deserialize<GenericEntity>(json);

            string reconstructedAddressJson = JsonSerializer.Serialize(reconstructedAddress);

            Assert.Equal(json, reconstructedAddressJson);
        }
    }
}