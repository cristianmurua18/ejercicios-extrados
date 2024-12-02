using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblio.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public int Edad { get; set; }


    }
}
