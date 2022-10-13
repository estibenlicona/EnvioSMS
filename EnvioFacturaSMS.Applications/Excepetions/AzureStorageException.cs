using System;
using System.Globalization;

namespace EnvioFacturaSMS.Applications.Excepetions
{
    public class AzureStorageException : Exception
    {
        public AzureStorageException() : base() { }

        public AzureStorageException(string Message) : base(Message) { }

        public AzureStorageException(string Message, string[] Args) : base(String.Format(CultureInfo.CurrentCulture, Message, Args)) { }
    }
}
