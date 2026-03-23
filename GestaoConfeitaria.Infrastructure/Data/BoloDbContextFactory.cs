using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GestaoConfeitaria.Infrastructure.Data
{
    internal class BoloDbContextFactory : IDesignTimeDbContextFactory<BoloDbContext>
    {
        public BoloDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "GestaoConfeitaria");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)                                  
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não foi encontrada no appsettings.json da API.");

            var optionsBuilder = new DbContextOptionsBuilder<BoloDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BoloDbContext(optionsBuilder.Options);
        }
    }
}
