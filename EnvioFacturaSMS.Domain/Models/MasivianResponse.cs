using System;

namespace EnvioFacturaSMS.Domain.Models
{
    public class MasivianResponse
    {
        public string DeliveryToken { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
