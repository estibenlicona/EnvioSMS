using System;
using System.Globalization;

namespace EnvioFacturaSMS.Applications.Excepetions
{
    public class ApiAbonoException : Exception
    {
        public ApiAbonoException() : base() { }

        public ApiAbonoException(string Message) : base(Message) { }

        public ApiAbonoException(string Message, string[] Args) : base(String.Format(CultureInfo.CurrentCulture, Message, Args)) { }
    }
}
