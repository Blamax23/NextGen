using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextGen.Dal.Context;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Dal
{
    public static class DbContextConfigurator
    {
        public static void AddNextGenDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<NextGenDbContext>(options =>
                options.UseSqlite(connectionString)); // Ou une autre base de données
        }
    }
}
