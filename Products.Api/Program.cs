using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Endpoints;
using Products.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x =>
{
    x.UseNpgsql(builder.Configuration["Postgres:ConnectionString"]);
});

builder.Services.AddScoped<CategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCategoryEndpoints();

app.UseHttpsRedirection();
app.Run();

























