using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using RadzenApp.Entities.QUVA;

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

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder)
        {
            //md Serilog
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}
