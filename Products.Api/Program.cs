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

        var connectionString = Environment.GetEnvironmentVariable("POSTGRES_LIVE_STRING");
        x.UseNpgsql(connectionString);
    }
});

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();



var app = builder.Build();

// Anropa automatiska migrations
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCategoryEndpoints();
app.MapProductEndpoints();

app.UseHttpsRedirection();
app.Run();