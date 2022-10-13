using EnvioFacturaSMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnvioFacturaSMS.Infraestructure.Configurations
{
    public class SessionTokenConfig : IEntityTypeConfiguration<SessionToken>
    {
        public void Configure(EntityTypeBuilder<SessionToken> builder)
        {
            builder.ToTable("SesionesUsuario");
            builder.HasKey(x => x.Token);
        }
    }
}
