using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using teste.inbev.core.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste.inbev.core.data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();                      
            builder.Property(c => c.Ativo).HasDefaultValue(true);
            builder.Property(c => c.Descricao).HasMaxLength(255);
        }
    }
}
