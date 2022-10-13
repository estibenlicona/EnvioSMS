using EnvioFacturaSMS.Domain.Entities;
using EnvioFacturaSMS.Infraestructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EnvioFacturaSMS.Infraestructure.Context
{
    public class PrivadaContext : DbContext
    {
        public DbSet<LogSMS> LogSMS { get; set; }
        public DbSet<Pagare> Pagare { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<Parametro> Parametro { get; set; }
        public DbSet<SessionToken> SessionToken { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Tienda> Tienda { get; set; }

        public PrivadaContext(DbContextOptions<PrivadaContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder == null)
            {
                return;
            }

            builder.ApplyConfiguration(new LogSMSConfig());
            builder.ApplyConfiguration(new PagareConfig());
            builder.ApplyConfiguration(new ContratoConfig());
            builder.ApplyConfiguration(new ParametroConfig());
            builder.ApplyConfiguration(new SessionTokenConfig());
            builder.ApplyConfiguration(new ClienteConfig());
            builder.ApplyConfiguration(new TiendaConfig());

            base.OnModelCreating(builder);
        }

    }
}
