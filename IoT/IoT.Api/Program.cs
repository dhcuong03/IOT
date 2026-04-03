using IoT.Api.Application.Services;
using IoT.Api.Infrastructure.Data;
using IoT.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Kết nối DB Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDb")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IDeviceConfigRepository, DeviceConfigRepository>();
builder.Services.AddScoped<IDeviceConfigService, DeviceConfigService>();
builder.Services.AddScoped<IIotDataRepository, IotDataRepository>();
builder.Services.AddScoped<IIotDataService, IotDataService>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertService, AlertService>();

builder.Services.AddScoped<IConnectionLogRepository, ConnectionLogRepository>();
builder.Services.AddScoped<IHeartbeatService, HeartbeatService>();


builder.Services.AddControllers();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();