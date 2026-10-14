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
        private static readonly IConfiguration _config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static string SageConn => _config.GetConnectionString("PACTRAK-Sage"); 
        public static string SqlConn => _config.GetConnectionString("PACTRAK-SQL"); 
        //        public static string SageConn = "Provider=VFPOLEDB.1;Data Source=Q:\\SBTPro\\PRODATA.DBC;Exclusive=No;Collate=Machine;NULL=NO;DELETED=NO;BACKGROUNDFETCH=NO;";
        //        public static string SageConn = "Provider=VFPOLEDB.1;Data Source=F:\\sage\\SBTPro\\PRODATA.DBC;Exclusive=No;Collate=Machine;NULL=NO;DELETED=NO;BACKGROUNDFETCH=NO;";
        //        public static string SqlConn = "Server=10.0.0.4;Database=Flight;User ID=FlightUser;Password=?Pass@word1;TrustServerCertificate=True;";
    }
}
