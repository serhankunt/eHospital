using DefaultCorsPolicyNugetPackage;
using eHospitalServer.Business;
using eHospitalServer.DataAccess;
using eHospitalServer.DataAccess.Context;
using eHospitalServer.Entities.Enums;
using eHospitalServer.Entities.Models;
using eHospitalServer.WebAPI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultCors();

builder.Services.AddBusiness();
builder.Services.AddDataAccess(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

ExtensionsMiddleware.CreateFirstUser(app);

app.UseHttpsRedirection();

app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    try
//    {
//        await next(context);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex);
//    }
//});

app.MapControllers()
    .RequireAuthorization(policy =>
    {
        // 404 hatasýný 401'e çeviriyor.
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.AddAuthenticationSchemes("Bearer");
    });

app.Run();
