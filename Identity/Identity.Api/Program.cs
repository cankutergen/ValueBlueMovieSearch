using Identity.Api.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

LoaderModule.ConfigureSwagger(builder);
LoaderModule.ConfigureMongo(builder);
LoaderModule.ConfigureServices(builder);
LoaderModule.ConfigureJwtService(builder);
LoaderModule.ConfigureLogger(builder);
LoaderModule.ConfigureAuthentication(builder);

var app = builder.Build();

await LoaderModule.CreateDefaultAdminUserAsync(app, builder);  

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
