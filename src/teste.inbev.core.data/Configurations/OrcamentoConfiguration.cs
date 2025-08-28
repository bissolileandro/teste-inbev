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
    public class OrcamentoConfiguration : IEntityTypeConfiguration<Orcamento>
    {
        public void Configure(EntityTypeBuilder<Orcamento> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(c => c.IdentificacaoCliente).HasMaxLength(20);
            builder.Property(c => c.NomeCliente).HasMaxLength(200);            
            builder.Property(c => c.Observacao).HasMaxLength(255);
            builder.Property(c => c.DescontoValor).HasColumnType("decimal(18,2)");
            builder.Property(c => c.DescontoPercentual).HasColumnType("decimal(18,2)");
            builder.Property(c => c.SubTotal).HasColumnType("decimal(18,2)");
            builder.Property(c => c.Total).HasColumnType("decimal(18,2)");
            builder.HasMany(o => o.OrcamentoItems)
                .WithOne(oi => oi.Orcamento)
                .HasForeignKey(oi => oi.OrcamentoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
