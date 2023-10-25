using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TMSBlazorAPI.Configuration;
using TMSBlazorAPI.Data;
using Microsoft.AspNetCore.Authentication;
using TMSBlazorAPI.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication("BasicAuthenticationHandler").AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler>("BasicAuthenticationHandler",null);
//Adding DB context !!!
var connString = builder.Configuration.GetConnectionString("TMSconn");
builder.Services.AddDbContext<TMSDbContext>(options => options.UseSqlServer(connString));
builder.Services.AddAutoMapper(typeof(MapperConfig));



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, lc)=> 
    lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration) );

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
