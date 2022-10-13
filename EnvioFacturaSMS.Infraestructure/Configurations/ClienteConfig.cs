using EnvioFacturaSMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnvioFacturaSMS.Infraestructure.Configurations
{
    public class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("cxc_cliente");
            builder.Property(x => x.Celular).HasColumnName("nce");
            builder.Property(x => x.Nombre).HasColumnName("nombre_1");
            builder.Property(x => x.Cedula).HasColumnName("cod_cli");
            builder.HasKey(x => x.Cedula);

        }
    }
}
