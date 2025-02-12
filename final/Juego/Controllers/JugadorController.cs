﻿using Entidades.DTOs.Cruds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicios.Servicios;

namespace Juego.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Jugador")]
    [ApiController]
    public class JugadorController(IJugadorServicio jugadorServicio) : ControllerBase
    {
        private readonly IJugadorServicio _jugadorServicio = jugadorServicio;

        #region Métodos exclusivos para Jugadores

        /// <summary>
        /// Debe poder registrar las cartas que tiene en su colección.
        /// Debe poder registrarse en un torneo, y armar un mazo de cartas con las cartas que cuenta para entrar a cada torneo.
        /// </summary>


        //PRIMERO CREO EL MAZO, me devuelve el numero de MazoID
        //Con una transaccion recupero ese MazoID necesario para registrar cartas
        [HttpPost("CrearMazo")]
        public async Task<IActionResult> CrearMazo(CrudMazoDTO mazo)
        {
            var res = await _jugadorServicio.CrearMazo(mazo);
            if (res > 0)
                Ok($"MazoID: {res}. Recuerdala");
            return BadRequest("No se posible crear el mazo.");

        }

        //Voy al registro de cartas y agrego las cartas en la tabla MazoCartas
        [HttpPost("RegistroCartas")]
        public async Task<IActionResult> RegistrarCartas(CrudMazoCartasDTO cartas)
        {
            return Ok(await _jugadorServicio.RegistrarCartas(cartas));

        }











        #endregion Fin Métodos de jugadores
    }
}
