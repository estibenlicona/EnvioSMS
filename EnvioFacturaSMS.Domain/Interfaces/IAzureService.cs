using System.IO;

namespace EnvioFacturaSMS.Domain.Interfaces
{
    public interface IAzureService
    {
        string Upload(byte[] BytesDocument, string name);
    }
}
