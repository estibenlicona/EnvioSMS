using EnvioFacturaSMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnvioFacturaSMS.Infraestructure.Configurations
{
    public class ParametroConfig : IEntityTypeConfiguration<Parametro>
    {
        public void Configure(EntityTypeBuilder<Parametro> builder)
        {
            builder.ToTable("ptv_vargen");
            builder.Property(x => x.Numero).HasColumnName("num_var");
            builder.Property(x => x.Nombre).HasColumnName("nom_var");
            builder.Property(x => x.Valor).HasColumnName("val_var");
            builder.HasKey(x => x.Numero);
        }
    }
}
