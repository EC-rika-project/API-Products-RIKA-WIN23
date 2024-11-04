using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Endpoints;
using Products.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>( x =>
{
    if (builder.Environment.IsDevelopment())
    {
        x.UseNpgsql(builder.Configuration["Postgres:ConnectionString"]);
    }
    else
    {
        var vUri = builder.Configuration["Vault:Uri"] ?? throw new ApplicationException("We are live and the vault URI is missing");
        var client = new SecretClient(new Uri(vUri), new DefaultAzureCredential());
        var secretName = builder.Configuration["Vault:PostgresSecret"] ?? throw new ApplicationException("We are live and the vault postgres whatever is missing");
        var secret = client.GetSecretAsync(secretName).GetAwaiter().GetResult();
        if (!secret.HasValue)
        {
            throw new ApplicationException("We are live and the secret for our db is missing");
        }
        x.UseNpgsql(secret.Value.Value);
    }
});

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCategoryEndpoints();
app.MapProductEndpoints();

app.UseHttpsRedirection();
app.Run();