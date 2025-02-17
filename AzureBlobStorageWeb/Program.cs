using Azure.Storage.Blobs;
using AzureBlobStorageWeb.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Register BlobServiceClient with connection string
builder.Services.AddSingleton(_ => new BlobServiceClient(configuration["AzureStorage:ConnectionString"]));
builder.Services.AddSingleton<BlobStorageService>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
