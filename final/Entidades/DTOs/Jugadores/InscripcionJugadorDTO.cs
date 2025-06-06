﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.DTOs.Jugadores
{
    public class InscripcionJugadorDTO
    {
        //public int UsuarioID { get; set; }
        public string? NombreApellido { get; set; }
        public string? Alias { get; set; }
        public int IdPaisOrigen { get; set; }
        public string? Email { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contraseña { get; set; }
        public string? FotoAvatar { get; set; }
        public string? Rol { get; set; }
        public int CreadoPor { get; set; }
        public bool Activo { get; set; }

    }
}
