using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teste.inbev.core.domain.Entities;

namespace teste.inbev.core.data.Configurations
{
    public class OrcamentoItemConfiguration : IEntityTypeConfiguration<OrcamentoItem>
    {
        public void Configure(EntityTypeBuilder<OrcamentoItem> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();                        
            builder.Property(c => c.Observacao).HasMaxLength(255);

            builder.HasOne(oi => oi.Produto)
                .WithMany()
                .HasForeignKey(oi => oi.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
