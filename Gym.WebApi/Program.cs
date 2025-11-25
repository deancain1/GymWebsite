using FluentValidation;
using Gym.Application;
using Gym.Application.Commands.Auth;
using Gym.Application.Interfaces;
using Gym.Domain.Entities;
using Gym.Infrastructure;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Services;
using Gym.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Services

builder.Services.AddCustomCors(builder.Configuration)
                .AddApplicationServices()
                .AddDatabaseAndIdentity(builder.Configuration)
                .AddJwtAuthentication(builder.Configuration)
                .AddSwaggerWithAuth()
                .AddEmailService(builder.Configuration);



builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();


app.UseApplicationPipeline();
app.Run();
