using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validaciones.Excepciones
{
    public class GeneralApiExcepcion : ExceptionBase
    {
        public GeneralApiExcepcion() : base() { }

        public GeneralApiExcepcion(string message) : base(message) { }

        public GeneralApiExcepcion(string mensaje, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, mensaje, args)) { }

        override public int GetStatusCode()
        {
            return StatusCodes.Status500InternalServerError;
        }
    }
}
