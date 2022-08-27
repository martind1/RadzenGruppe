using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Oracle.EntityFrameworkCore.Infrastructure;
using RadzenApp.Entities.QUVA;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;

namespace RadzenApp.Connection.QUVA
{
    public partial class QuvaContext
    {
        //md Serilog
        private readonly ILoggerFactory _loggerFactory;

        public QuvaContext(DbContextOptions<QuvaContext> options, ILoggerFactory loggerFactory) :
            base(options)
        {
            //md Serilog
            _loggerFactory = loggerFactory; 
            OnCreated();
        }

        private static string GetOracleSQLCompatibility()
        {
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            var configuration = configurationBuilder.Build();
            return configuration["OracleSQLCompatibility"];  //11,12
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder)
        {
            //md Oracle
            //Action<OracleDbContextOptionsBuilder> oracleOptionsAction = b => b.UseOracleSQLCompatibility("11");
            //oracleOptionsAction.Invoke(new OracleDbContextOptionsBuilder(optionsBuilder));
            //optimiert:
            void oracleOptionsAction(OracleDbContextOptionsBuilder b) => b.UseOracleSQLCompatibility(GetOracleSQLCompatibility());
            oracleOptionsAction(new OracleDbContextOptionsBuilder(optionsBuilder));

            //md Serilog
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}
