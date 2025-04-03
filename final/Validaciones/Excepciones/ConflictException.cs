using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validaciones.Excepciones
{
    public class ConflictException : ExceptionBase
    {
        public ConflictException() : base() { }

        public ConflictException(string message) : base(message) { }

        public ConflictException(string mensaje, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, mensaje, args)) { }

        override public int GetStatusCode()
        {
            return StatusCodes.Status409Conflict;
        }
    }
}
