using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; 
using System.IO; 

namespace teste.inbev.core.data.Context
{
    public class InbevCoreContextFactory : IDesignTimeDbContextFactory<InbevCoreContext>
    {
        public InbevCoreContext CreateDbContext(string[] args)
        {
            
            IConfigurationRoot configuration = new ConfigurationBuilder()                
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<InbevCoreContext>();
            var connectionString = configuration.GetConnectionString("InbevCoreContext");
            
            builder.UseSqlServer(connectionString);

            return new InbevCoreContext(builder.Options);
        }
    }
}