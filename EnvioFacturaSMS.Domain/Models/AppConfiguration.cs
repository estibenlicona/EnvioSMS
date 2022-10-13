namespace EnvioFacturaSMS.Domain.Models
{
    public class AppConfigurations
    {

        public string ApiExportHtml { get; set; }
        public string AzureStorage { get; set; }
        public EnvioSMSConfiguration EnvioSMS { get; set; }
        

    }

    public class AzureStorageConfiguration
    {
        public string ConnectionString { get; set; }
        public string Container { get; set; }

    }

    public class EnvioSMSConfiguration
    {
        public string Url { get; set; }
    }

    public class Enpoints
    {
        public string Abono { get; set; }
    }


}

