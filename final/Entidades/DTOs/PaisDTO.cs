using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs
{
    public class PaisDTO
    {

        public Nombre? Name { get; set; }

        //public List<string>? Timezones { get; set; }

        public List<string>? Timezones { get; set; }
      

    }

    public class Nombre
    {
        public string? common { get; set; }


        public string? official { get; set; }


    }





}
