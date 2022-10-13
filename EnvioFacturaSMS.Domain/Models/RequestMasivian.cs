namespace EnvioFacturaSMS.Domain.Models
{
    public class RequestMasivian
    {
        public string To { get; set; }
        public string Text { get; set; }
        public bool LongMessage { get; set; }
        public bool IsPremium { get; set; }
        public bool IsFlash { get; set; }
        public string Url { get; set; }
    }
}
