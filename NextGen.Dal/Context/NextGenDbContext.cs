using NextGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NextGen.Dal.Mapping;

namespace NextGen.Dal.Context
{
    public class NextGenDbContext : DbContext
    {
        public NextGenDbContext(DbContextOptions<NextGenDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Actualite> Actualites { get; set; }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Appeler la configuration pour User
            modelBuilder.ApplyConfiguration(new UserMap());

            // Si tu as d'autres configurations, tu peux les appeler ici aussi
            modelBuilder.ApplyConfiguration(new ActualiteMap());

            modelBuilder.ApplyConfiguration(new SourceMap());
            modelBuilder.ApplyConfiguration(new QuestionMap());
        }
    }
}
