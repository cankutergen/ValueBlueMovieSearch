using AutoMapper;
using FluentValidation;
using MongoDB.Driver;
using SearchLog.Api.Configurations;
using SearchLog.Business.Abstract;
using SearchLog.Business.Concrete;
using SearchLog.Business.ValidationRules;
using SearchLog.DataAccess.Abstract;
using SearchLog.DataAccess.Concrete.Mongo;
using SearchLog.Entities.Concrete;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

LoaderModule.ConfigureSwagger(builder);
LoaderModule.ConfigureMongo(builder);
LoaderModule.ConfigureServices(builder);
LoaderModule.ConfigureLogger(builder);
LoaderModule.ConfigureAuthentication(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

