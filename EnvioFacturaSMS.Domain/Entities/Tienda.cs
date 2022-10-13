namespace EnvioFacturaSMS.Domain.Entities
{
    public class Tienda : BaseEntity
    {
        public string TiendaId { get; set; }
        public short IndicadorEnvio { get; set; }
    }
}
