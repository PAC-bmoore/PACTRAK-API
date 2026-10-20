using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using PAC.Web.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

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
