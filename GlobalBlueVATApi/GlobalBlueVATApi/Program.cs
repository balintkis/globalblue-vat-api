using GlobalBlueVATApi.Repository;
using GlobalBlueVATApi.Service;
using GlobalBlueVATApi.Service.Austria;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IService, Service>();
builder.Services.AddSingleton<IAustriaServiceHelper, AustriaServiceHelper>();
builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddSingleton<IAustrianVatRateValidator, AustrianVatRateValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
