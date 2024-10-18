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
    public class QuestionMap : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Question");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd().HasColumnName("Id");

            builder.Property(p => p.Intitule).IsRequired().HasMaxLength(150).HasColumnName("Intitule");
            builder.Property(p => p.Reponse).HasMaxLength(1000).HasColumnName("Reponse");
        }
    }
}
