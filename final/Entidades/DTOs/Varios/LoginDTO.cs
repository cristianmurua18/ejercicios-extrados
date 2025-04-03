using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Varios
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [MinLength(5, ErrorMessage = "Minimo 5 caracteres")]
        [StringLength(200, ErrorMessage = "Maximo 200 caracteres")]
        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [StringLength(300, ErrorMessage = "Maximo 300 caracteres")]
        public string? Contraseña { get; set; }
     


    }
}
