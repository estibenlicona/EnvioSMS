using System;

namespace EnvioFacturaSMS.Domain.Models
{
    public class SmsAbono
    {
        public string NumeroDocumento { get; set; }
        public string Cedula { get; set; }
        public string Celular { get; set; }
        public string Mensaje { get; set; }
        public short NumeroParametro { get; set; }
        public DateTime TimeStamp { get; protected set; }

        public SmsAbono()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
