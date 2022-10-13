using EnvioFacturaSMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnvioFacturaSMS.Infraestructure.Configurations
{
    public class LogSMSConfig : IEntityTypeConfiguration<LogSMS>
    {
        public void Configure(EntityTypeBuilder<LogSMS> builder)
        {
            builder.ToTable("AppLogs_sms");
            builder.Property(x => x.Celular).HasColumnName("nro_celular");
            builder.HasKey(x => x.Id);
        }
    }
}
