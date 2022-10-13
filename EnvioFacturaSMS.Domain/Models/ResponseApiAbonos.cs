namespace EnvioFacturaSMS.Domain.Models
{
    public class ResponseApiAbonos
    {
        public string Resultado { get; set; }
        public byte[] Factura { get; set; }
    }
}
