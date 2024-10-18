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
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Utilisateur");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnName("Id");

            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(50).HasColumnName("Prenom");
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(50).HasColumnName("Nom");
            builder.Property(p => p.Email).IsRequired().HasMaxLength(100).HasColumnName("Email");
            builder.Property(p => p.Password).IsRequired().HasMaxLength(500).HasColumnName("Password");
            builder.Property(p => p.OldPassword).IsRequired().HasMaxLength(500).HasColumnName("OldPassword");
            builder.Property(p => p.IsAdmin).IsRequired().HasColumnName("IsAdmin");
            builder.Property(p => p.AccepteNews).IsRequired().HasColumnName("AccepteNews");
        }

    }
}
