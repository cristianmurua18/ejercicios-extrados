using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validaciones.Excepciones
{
    public class ServiceNotAvailableException : ExceptionBase
    {
        public ServiceNotAvailableException() : base() { }

        public ServiceNotAvailableException(string message) : base(message) { }

        public ServiceNotAvailableException(string mensaje, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, mensaje, args)) { }

        override public int GetStatusCode()
        {
            return StatusCodes.Status503ServiceUnavailable;
        }
    }
}
