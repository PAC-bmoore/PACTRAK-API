using Azure.Identity;
using DotNetEnv;
using DotNetEnv.Configuration;
using PAC.DataLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

string envFile = Path.Combine(builder.Environment.ContentRootPath, ".env");
if (File.Exists(envFile))
    builder.Configuration.AddDotNetEnv(envFile, LoadOptions.NoEnvVars());

string? keyVaultUrl = builder.Configuration["KeyVault:Url"];
if (!string.IsNullOrEmpty(keyVaultUrl))
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new ManagedIdentityCredential());

BaseDL.Init(builder.Configuration);

var sqlConnStr = builder.Configuration.GetConnectionString("PACTRAK-SQL");
var sageConnStr = builder.Configuration.GetConnectionString("PACTRAK-Sage");

// This service is reached only from PAC.Web.API on the web server, which
// holds the Entra gate. The token scheme stays for the controllers that
// ask for it ([Authorize] on Customer).
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Property names go out as written in the classes (PascalCase), the shape
// the portal reads on the tracking routes. The proxy reads the other
// routes case-insensitively, so nothing changes for them.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddProblemDetails();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// an unhandled exception becomes a plain problem response, never the
// exception itself
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "up" }));
app.MapControllers();

app.Run();
