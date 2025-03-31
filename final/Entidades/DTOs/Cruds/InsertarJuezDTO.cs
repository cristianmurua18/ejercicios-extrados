using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Cruds
{
    public class InsertarJuezDTO
    {

        [Required(ErrorMessage = "El nombre y el apellido son requeridos.")]
        [DataType(DataType.Text)]
        [MinLength(5, ErrorMessage = "Minimo 5 caracteres")]
        [StringLength(150, ErrorMessage = "Maximo 150 caracteres")]
        public string? NombreApellido { get; set; }

        [Required(ErrorMessage = "El alias es requerido.")]
        [MinLength(3, ErrorMessage = "Minimo 3 caracteres")]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        public string? Alias { get; set; }

        [Required(ErrorMessage = "El IdPais es requerido.")]
        [Range(1, 250, ErrorMessage = "Debe estar entre 1 y 250")]
        public int IdPaisOrigen { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El correo es invalido, revise formato")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [MinLength(5, ErrorMessage = "Minimo 5 caracteres")]
        [StringLength(200, ErrorMessage = "Maximo 200 caracteres")]
        public string? NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(8, ErrorMessage = "Minimo 8 caracteres")]
        [StringLength(300, ErrorMessage = "Maximo 300 caracteres")]
        public string? Contraseña { get; set; }

        [Required(ErrorMessage = "El rol es requerido.")]
        [AllowedValues("Administrador", "Organizador", "Juez", "Jugador")]
        [DataType(DataType.Text)]
        public string? Rol { get; set; }

        [Required(ErrorMessage = "Activo 1 o de baja 0 es requerido.")]
        [Range(0, 1, ErrorMessage = "Debe ser 1 Activo o 0 de baja")]
        public bool Activo { get; set; } = true;
    }
}
