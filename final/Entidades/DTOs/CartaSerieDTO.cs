using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs
{
    public class CartaSerieDTO
    {
        //public List<Types>? Types { get; set; }
        public int id {  get; set; }
        public List<TypeInfo> types { get; set; }

    }

    public class TypeInfo
    {
        //public int slot { get; set; }
        public TypeData? type { get; set; }
    }

    public class TypeData
    {
        public string? name { get; set; }
        //public string url { get; set; }
    }
    //public class Types
    //{
    //    public Type? Type { get; set; }

    //}

    //public class Type
    //{
    //    public List<string>? Name { get; set; }
    //}
}
