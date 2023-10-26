using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TMSBlazorAPI.Configuration;
using TMSBlazorAPI.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TMSBlazorAPI.Handler;
using TMSBlazorAPI.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//basic authentication
//builder.Services.AddAuthentication("BasicAuthenticationHandler").AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHandler>("BasicAuthenticationHandler",null);

var _authkey = builder.Configuration.GetValue<string>("JwtSettings:securitykey");
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Adding DB context !!!
var connString = builder.Configuration.GetConnectionString("TMSconn");
builder.Services.AddDbContext<TMSDbContext>(options => options.UseSqlServer(connString));
builder.Services.AddAutoMapper(typeof(MapperConfig));

var _JwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>( _JwtSettings );

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
