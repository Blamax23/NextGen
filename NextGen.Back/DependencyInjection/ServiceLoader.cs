using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NextGen.Dal;

namespace NextGen.Back.DependencyInjection
{
    public static class ServiceLoader
    {
        public static void LoadServices(this IServiceCollection services, string connectionString)
        {
            services.AddNextGenDbContext(connectionString); // Appelle la configuration du DbContext
            // Ajoute d'autres services ici
        }
    }
}
