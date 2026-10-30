using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace PAC.DataLayer
{
    public class BaseDL 
    {
        private static IConfiguration _config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static void Init(IConfiguration config)
        {
            _config = config;
        }

        public static string SageConn => _config.GetConnectionString("PACTRAK-Sage"); 
        public static string SqlConn => _config.GetConnectionString("PACTRAK-SQL");
        public static string BaseAPIUrl => _config["WebSettings:ApiBaseUrl"];
    }
}
