using AzureBlobAPI.Helpers;
using AzureBlobAPI.Presents;
using AzureBlobAPI.Services;
using AzureBlobAPI.Settings;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BlobSettings>(
    builder.Configuration.GetSection(BlobSettings.BlobStotage));

builder.Services.AddScoped<IBlobStorage, BlobStorage>();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["default:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["default:queue"], preferMsi: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.BlobEndpoint();

app.UseHttpsRedirection();

app.Run();