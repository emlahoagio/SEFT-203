using AutoMapper;
using Contracts;
using Entities.AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Repo;
using WebAPI.Extentions;
using Microsoft.OpenApi.Models;

// AutoMapper config
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfile());
});

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region quantmdo Config
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
NLog.LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); // NLog config
builder.Services.ConfigurePostgres(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureLoggerService();
builder.Services.AddSingleton(config.CreateMapper()); // AutoMapper config
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddAuthentication(); // Config identity
builder.Services.ConfigureIdentity(); // Config identity
builder.Services.ConfigureJWT(builder.Configuration); // config JWT
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>(); // DI
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Token Authentication API",
        Description = "ASP.NET Core 6 Web API"
    });
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },new string[] {}
        }
    });
});

var app = builder.Build();

#region quantmdo config
app.UseCors("CorsPolicy");
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthentication(); // config identity, jwt

app.UseAuthorization();

app.MapControllers();

app.Run();
