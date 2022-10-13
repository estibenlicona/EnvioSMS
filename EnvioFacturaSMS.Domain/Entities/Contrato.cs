using System;

namespace EnvioFacturaSMS.Domain.Entities
{
    public class Contrato : BaseEntity
    {
        public int LogId { get; set; }
        public string NumeroContrato { get; set; }
        public string Url { get; set; }
        public string Cedula { get; set; }
        public int EnvioSms { get; set; }
        public DateTime Fecha { get; set; }
        public string TiendaId { get; set; }

    }
}
