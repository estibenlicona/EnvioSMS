using System;

namespace EnvioFacturaSMS.Domain.Entities
{
    public class Pagare : BaseEntity
    {
        public int LogId { get; set; }
        public string NumeroPagare { get; set; }
        public string Url { get; set; }
        public string Cedula { get; set; }
        public int EnvioSms { get; set; }
        public DateTime Fecha { get; set; }
        public string TiendaId { get; set; }
    }
}
