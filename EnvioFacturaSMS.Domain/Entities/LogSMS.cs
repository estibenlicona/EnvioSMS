using System;

namespace EnvioFacturaSMS.Domain.Entities
{
    public class LogSMS : BaseEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Celular { get; set; }
        public string Proceso { get; set; }
    }
}
