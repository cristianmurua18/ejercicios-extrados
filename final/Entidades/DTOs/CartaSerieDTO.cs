using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs
{
    public class CartaSerieDTO
    {
        public List<Types>? Types { get; set; }
    }

    public class Types
    {
        public Type? Type { get; set; }

    }

    public class Type
    {
        public List<string>? Name { get; set; }
    }
}
