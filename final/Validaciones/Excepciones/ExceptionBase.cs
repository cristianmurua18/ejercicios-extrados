using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validaciones.Excepciones
{
    public class ExceptionBase : Exception
    {
        public ExceptionBase() : base() { }

        public ExceptionBase(string mensaje) : base(mensaje) { }

        public ExceptionBase(string mensaje, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, mensaje, args)) { }

        virtual public int GetStatusCode()
        {
            return StatusCodes.Status500InternalServerError;
        }

    }
}
