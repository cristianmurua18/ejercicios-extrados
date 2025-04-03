using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validaciones.Excepciones
{
    public class UnauthenticatedException : ExceptionBase
    {
        public UnauthenticatedException() : base() { }

        public UnauthenticatedException(string message) : base(message) { }

        public UnauthenticatedException(string mensaje, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, mensaje, args)) { }

        override public int GetStatusCode()
        {
            return StatusCodes.Status401Unauthorized;
        }
    }
}
