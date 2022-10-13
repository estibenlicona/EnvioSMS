namespace EnvioFacturaSMS.Domain.Entities
{
    public class Parametro : BaseEntity
    {
        public short Numero { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}
