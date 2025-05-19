using ArsalanApp;
using ArsalanApp.Data;
using ArsalanApp.Models;
using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SignalRServer;


// https://andrewlock.net/exploring-the-dotnet-8-preview-introducing-the-identity-api-endpoints/
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-8.0

var builder = WebApplication.CreateBuilder(args);

// Add console logging
builder.Logging.AddConsole();

// Add Authorizations
builder.Services.AddAuthorization();

// Add HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Add Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add Identity Endpoints
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Add Signal R
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

#region Swagger
// Add benchmark
var summary = BenchmarkRunner.Run<WeatherForecast>;
Console.WriteLine(summary);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

//    // Add Security Definition
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter into field the word 'Bearer' followed by a space of your token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });

//    // Add Security Requirement
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                {
//                    new OpenApiSecurityScheme
//                    {
//                        Reference = new OpenApiReference
//                        {
//                            Type = ReferenceType.SecurityScheme,
//                            Id = "Bearer"
//                        }
//                    },
//                    Array.Empty<string>()
//                }
//            });
//});

builder.Services.AddSwaggerGen(opt =>
{
    opt.EnableAnnotations();

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Description = "Please enter into field the word 'Bearer' followed by a space of your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
});

#endregion

var app = builder.Build();

#region Swagger app setting
//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseSwagger(opt =>
{
    opt.RouteTemplate = "openapi/{documentName}.json";

});
app.MapScalarApiReference(options =>
{
    options.Title = "Api Testing with Scalar";
    options.Theme = ScalarTheme.BluePlanet;
    options.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    //options.WithModels(false);
    options.Layout = ScalarLayout.Modern;
    options.Authentication = new ScalarAuthenticationOptions();
});
#endregion

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Add Identity Api
app.MapGroup("api/auth")
    .MapIdentityApi<AppUser>();

// Add Signal R
app.MapHub<NotificationsHub>("/notifications");

app.MapControllers();

app.Run();
