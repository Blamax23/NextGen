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
    public class SourceMap : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.ToTable("Source");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnName("Id");

            builder.Property(p => p.Path).IsRequired().HasMaxLength(255).HasColumnName("Chemin");
            builder.Property(p => p.IdActualite).HasColumnName("IdActualite");
            builder.Property(p => p.Name).HasMaxLength(150).HasColumnName("Nom");
            builder.Property(p => p.Type).HasMaxLength(50).HasColumnName("Type");
        }
    }
}
