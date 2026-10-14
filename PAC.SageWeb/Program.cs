using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using PAC.SageWeb.Auth;

var builder = WebApplication.CreateBuilder(args);

var sqlConnStr = builder.Configuration.GetConnectionString("PACTRAK-SQL");
var sageConnStr = builder.Configuration.GetConnectionString("PACTRAK-Sage");

// Two ways in: an Entra token for this API's app registration, or an
// issued API key (ApiKeys section). Every endpoint requires one of them;
// only /health is open, for the host's probe.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationHandler.SchemeName, null);
builder.Services.AddAuthorization(options =>
{
    var caller = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, ApiKeyAuthenticationHandler.SchemeName)
        .RequireAuthenticatedUser()
        .Build();
    options.DefaultPolicy = caller;
    options.FallbackPolicy = caller;
});

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
}

// an unhandled exception becomes a plain problem response, never the
// exception itself
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new { status = "up" })).AllowAnonymous();
app.MapControllers();

app.Run();
