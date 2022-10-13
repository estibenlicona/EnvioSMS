using System.Collections.Generic;

namespace EnvioFacturaSMS.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Celular { get; set; }
    }
}
