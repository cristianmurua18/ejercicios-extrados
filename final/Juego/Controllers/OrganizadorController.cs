﻿using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.Servicios.Organizador;

namespace Juego.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Organizador")]
    [ApiController]
    public class OrganizadorController(IOrganizadorServicio organizadorServicio) : ControllerBase
    {

        private readonly IOrganizadorServicio _organizadorServicio = organizadorServicio;


        #region Métodos exclusivos para Organizadores
        /// <summary>
        /// Pueden crear, editar y cancelar torneos
        /// Pueden hacer avanzar un torneo a la siguiente fase, y modificar las partidas del torneo
        /// Los debe crear un Administrador, pueden crear Jueces
        /// </summary>

        /// <summary>
        /// Metodo para obtener usuarios por un rol especifico y que fueron creados por el usuario autenticado
        /// </summary>
        [HttpGet("ObtenerUsuariosPorRol")]
        public async Task<IActionResult> VerListadoUsuarios(string rol)
        {
            if (rol != "Jugador" || rol != "Juez")
                return BadRequest("Solo puedes ver Jugadores o jueces creados por ti");
            //RIVISAR que el usuario fue creado por el
            return Ok(await _organizadorServicio.VerListadoUsuarios(rol));

        }

        /// <summary>
        /// Metodo para registrar jueces. Solo usuarios con rol Juez
        /// </summary>
        [HttpPost("RegistroJuez")]
        public async Task<IActionResult> RegistrarJuez(CrudUsuarioDTO usuario)
        {
            //Verificacion Inicial para que solo pueda crear Jueces, en que sea su torneo organizado?. 
            if (usuario.Rol == "Juez")
            {
                //Validaciones basicas
                if (usuario == null) return BadRequest("No se agrego ningun usuario");
                //Si un modelo no es valido, valida estado del formulario, si alguna validacion no se cumple
                if (!ModelState.IsValid) return BadRequest(ModelState);
                //devolver un NoContent()?
                return Ok(await _organizadorServicio.RegistrarJuez(usuario));
                //FUNCIONO
            }
            return BadRequest("No es posible insertar otro tipo de usuario");

        }

        /// <summary>
        /// Metodo para crear un torneo. Fecha fin se puede no insertar y jugador ganador igual, hasta conocer el resultado
        /// </summary>
        [HttpPost("CrearTorneo")]
        public async Task<IActionResult> CrearTorneo(CrudTorneoDTO torneo)
        {
            var respon = await _organizadorServicio.CrearTorneo(torneo);
            return respon ? Ok($"Registro exitoso") : BadRequest("No fue posible insertar");
            //FUNCIONAAA
        }

        /// <summary>
        /// Metodo para crear la lista de series que acepta un torneo
        /// </summary>
        [HttpPost("CrearTorneoSerieHabilitada")]
        public async Task<IActionResult> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie)
        {
            var resp = await _organizadorServicio.CrearTorneoSerieHabilitada(serie);

            return resp ? Ok($"Registro exitoso") : BadRequest("No fue posible insertar");

        }

        /// <summary>
        /// Metodo para editar un torneo. Fecha fin se puede no insertar y jugador ganador, hasta conocer el resultado
        /// </summary>
        [HttpPut("EditarTorneoById")]
        public async Task<IActionResult> EditarTorneoById(CrudTorneoDTO torneo)
        {
            //Ver que sean solo sus torneos organizados
            //Sirve para hacer avanzar a la siguiente fase un torneo
            return Ok(await _organizadorServicio.EditarTorneo(torneo));
        }

        /// <summary>
        /// Metodo para cancelar un torneo. Se cambia el estado a Cancelado.
        /// </summary>
        [HttpPut("CancelarTorneo")]
        public async Task<IActionResult> CancelarTorneo(int torneoid, string estado)
        {
            //Ver que sean solo sus torneos organizados
            return Ok(await _organizadorServicio.CancelarTorneo(torneoid, estado));

        }

        /// <summary>
        /// Metodo para crear rondas. -- Asignaciones: 6 (64avos), 5 (32avos), 4 (16avos), 3 (8vos), 2 (4tos), 1 (semifinal), 0 (final)
        /// </summary>
        [HttpPost("CrearRondasYPartidas")]
        public async Task<IActionResult> CrearRondasYPartidas(int idTorneo)
        {
            await _organizadorServicio.GenerarRondasYPartidas(idTorneo);
            //Ver otro retorno
            return Ok("Rondas y partidas creadas con exito");
        }

        /// <summary>
        /// Metodo para crear partidas. -- Asignaciones: 6 (64avos), 5 (32avos), 4 (16avos), 3 (8vos), 2 (4tos), 1 (semifinal), 0 (final)
        /// </summary>
        //[HttpPost("CrearRondas")]
        //public async Task<IActionResult> CrearPartidas(PartidaDTO partida)
        //{
        //    return Ok(await _usuarioServicio.CrearPartidas(partida));
        //}


        ////A lo mejor falta un metodo para crear, modificar partidas
        //[HttpPost("ModificarPartida")]
        //public async Task<IActionResult> ModificarPartida(PartidaDTO partida)
        //{
        //    //modificar partidas solo creadas por el
        //    return Ok(await _usuarioServicio.ModificarPartida(partida));
        //}



        #endregion Final Organizadores
    }
}
