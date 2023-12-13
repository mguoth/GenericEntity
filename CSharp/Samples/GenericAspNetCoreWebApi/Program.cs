using Org.GenericEntity.Abstractions;
using Org.GenericEntity.Extensions.FileSystem;
using Org.GenericEntity.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISchemaRepository>(new FileSystemSchemaRepository("Schemas"));

//Required for deserialisation
GenericEntity.Extensions.AddFileSystemSchemaRepository();
GenericEntity.Extensions.AddAvroSchema();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
