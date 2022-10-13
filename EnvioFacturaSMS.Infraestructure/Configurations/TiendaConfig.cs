using EnvioFacturaSMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnvioFacturaSMS.Infraestructure.Configurations
{
    public class TiendaConfig : IEntityTypeConfiguration<Tienda>
    {
        public void Configure(EntityTypeBuilder<Tienda> builder)
        {
            builder.ToTable("inv_bodegas");
            builder.Property(x => x.IndicadorEnvio).HasColumnName("IndEnvioSMSPagCon");
            builder.Property(x => x.TiendaId).HasColumnName("cod_bod");
            builder.HasKey(x => x.TiendaId);
            
        }
    }
}
