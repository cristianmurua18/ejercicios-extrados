﻿using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.DTOs.Jugadores;
using Entidades.DTOs.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Acceso
{
    public interface IAccesoServicio
    {
        //Definicion de Metodos
        public Task<bool> ObtenerPokemones();
        public Task<bool> RellenarCartaSerie();
        public Task<List<RespuestaPaisDTO>> ObtenerIdPais(string nombre);
        public Task<bool> RegistroJugador(CrudUsuarioDTO jugador);
        public Task<AutorizacionRespuestaDTO> ObtenerAutenticacion(LoginDTO login);


    }
}
