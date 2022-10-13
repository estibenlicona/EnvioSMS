using EnvioFacturaSMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnvioFacturaSMS.Infraestructure.Configurations
{
    public class PagareConfig : IEntityTypeConfiguration<Pagare>
    {
        public void Configure(EntityTypeBuilder<Pagare> builder)
        {
            builder.ToTable("cxc_firma_electronica_log");
            builder.Property(x => x.Url).HasColumnName("ruta_pdf");
            builder.Property(x => x.NumeroPagare).HasColumnName("num_pagare");
            builder.Property(x => x.Cedula).HasColumnName("cedula");
            builder.Property(x => x.EnvioSms).HasColumnName("sms_envio");
            builder.Property(x => x.Fecha).HasColumnName("fecha");
            builder.Property(x => x.TiendaId).HasColumnName("bodega");
            builder.HasKey(x => x.LogId);
        }
    }
}
