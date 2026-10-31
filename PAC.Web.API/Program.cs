using Azure.Identity;
using DotNetEnv;
using DotNetEnv.Configuration;
using PAC.DataLayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using PAC.Web.API.Services;

const string PacTenantId = "d341c7d5-fe3e-4c64-9fc2-fdb9cb6a231d";

var builder = WebApplication.CreateBuilder(args);

string envFile = Path.Combine(builder.Environment.ContentRootPath, ".env");
if (File.Exists(envFile))
    builder.Configuration.AddDotNetEnv(envFile, LoadOptions.NoEnvVars());

string? keyVaultUrl = builder.Configuration["KeyVault:Url"];
if (!string.IsNullOrEmpty(keyVaultUrl))
    builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new ManagedIdentityCredential());

BaseDL.Init(builder.Configuration);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Cargo.Read", policy =>
    {
        policy.RequireRole("Cargo.Read");
        policy.RequireAssertion(context => context.User.HasClaim("tid", PacTenantId) || context.User.HasClaim("http://schemas.microsoft.com/identity/claims/tenantid", PacTenantId));
    });
    options.AddPolicy("Customer.Write", policy =>
    {
        policy.RequireRole("Customer.Write");
        policy.RequireAssertion(context => context.User.HasClaim("tid", PacTenantId) || context.User.HasClaim("http://schemas.microsoft.com/identity/claims/tenantid", PacTenantId));
    });
});

var sqlConnStr = builder.Configuration.GetConnectionString("PACTRAK-SQL");
var sageConnStr = builder.Configuration.GetConnectionString("PACTRAK-Sage");

// The tracking routes forward to PAC.SageWeb byte for byte (see
// Services/InnerApi.cs); the other controllers keep their RestSharp calls.
builder.Services.AddHttpClient<InnerApi>(client =>
{
    var baseUrl = builder.Configuration["WebSettings:ApiBaseUrl"]
        ?? throw new InvalidOperationException("WebSettings:ApiBaseUrl is not set.");
    client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
