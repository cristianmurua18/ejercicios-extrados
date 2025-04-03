using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validaciones.Excepciones
{
    public class ItemNotFoundException : ExceptionBase
    {

        public ItemNotFoundException() : base() { }
        public ItemNotFoundException(string message) : base(message) { }

        public ItemNotFoundException(string mensaje, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, mensaje, args)) { }

        override public int GetStatusCode()
        {
            return StatusCodes.Status404NotFound;
        }

    }
}
