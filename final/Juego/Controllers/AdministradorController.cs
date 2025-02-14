using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System;
using Utilidades.Utilidades;
using System.Diagnostics.Eventing.Reader;
using FluentValidation;
using Validaciones.Validacion;
using Entidades.DTOs.Cruds;
using Servicios.Servicios.Administrador;

namespace Juego.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    [ApiController]
    public class AdministradorController(IAdministradorServicio administradorServicio) : ControllerBase
    {
        private readonly IAdministradorServicio _administradorServicio = administradorServicio;

        #region Métodos exclusivos para Administradores
        /// <summary>
        /// Pueden crear, editar y eliminar otros administradores, organizadores, jueces y jugadores (Usuarios)
        /// Pueden ver torneos y cancelarlos
        /// </summary>

        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuariosPorNombre(string nombre)
        {
            //return StatusCode(StatusCodes.Status200OK, new { Value = _usuarioServicio.ObtenerUsuarios() });
            return Ok(new { Usuarios = await _administradorServicio.ObtenerUsuariosPorNombre(nombre) });
            //FUNCIONA, agregar paginacion
        }

        [HttpGet("ObtenerUsuario/{id:int}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            var user = await _administradorServicio.ObtenerUsuarioPorId(id);

            return user != null ? Ok(user): NotFound("Usuario no encontrado");

        }
        //FUNCIONA sin autenticacion. SEGUIR PROBANDO

        [HttpPost("RegistroUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] CrudUsuarioDTO usuario)
        {
            //Ver como implementar la validacion
            if (await _administradorServicio.RegistrarUsuario(usuario))
            {   //Validaciones basicas
                if (usuario == null) return BadRequest("Usuario no puede ser null");
                //Si un modelo no es valido, valida estado del formulario, si alguna validacion no se cumple
                if (!ModelState.IsValid) return BadRequest(ModelState);
                return Ok("Registro exitoso");
            }
            return BadRequest("Registro fallido. Revisar informacion");
            //REVISAR
        }

        [HttpPut("ActualizarUsuarioPorId")]
        public async Task<IActionResult> ActualizarUsuarioPorID(CrudUsuarioDTO usuario)
        {
            return Ok(await _administradorServicio.ActualizarUsuarioPorID(usuario));
        }

        [HttpDelete("BorrarUsuarioPorId")]
        public async Task<IActionResult> BorrarUsuarioPorID(CrudUsuarioDTO usuario)
        {
            return Ok(await _administradorServicio.BorrarUsuarioPorID(usuario)); //Borrado logico
        }

        [HttpGet("VerTorneos")]
        public async Task<IActionResult> VerTorneosYPartidas()
        {
            return Ok(await _administradorServicio.VerTorneosYpartidas());
            //Funciona con Exito, ver paginacion
        }

        //Metodo para 

        [HttpPost("CancelarTorneo")]
        public async Task<IActionResult> CancelarTorneos(int torneoid, string texto)
        {
            return Ok(await _administradorServicio.CancelarTorneos(torneoid, texto));
        }

        #endregion Final Administradores




    }
}
