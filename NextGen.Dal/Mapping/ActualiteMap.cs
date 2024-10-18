using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NextGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextGen.Dal.Mapping
{
    public class ActualiteMap : IEntityTypeConfiguration<Actualite>
    {
        public void Configure(EntityTypeBuilder<Actualite> builder)
        {
            builder.ToTable("Actualite");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnName("Id");

            builder.Property(p => p.Titre).IsRequired().HasMaxLength(150).HasColumnName("Titre");
            builder.Property(p => p.Contenu).HasMaxLength(1000).HasColumnName("Contenu");
            builder.Property(p => p.DateCreation).IsRequired().HasColumnName("DateCreation");
            builder.Property(p => p.DateModification).IsRequired().HasColumnName("DateModif");
            builder.Property(p => p.IdUtilisateur).HasColumnName("IdUtilisateur");
        }
    }
}
