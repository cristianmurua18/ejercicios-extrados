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
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet("ObtenerUsuariosRol")]
        public async Task<IActionResult> ObtenerUsuariosPorRol(string rol)
        {
            var lista = await _administradorServicio.ObtenerUsuariosPorRol(rol);

            if (lista.IsNullOrEmpty())
                return NotFound("Usuario no encontrado.");
            return Ok(new { Usuarios = lista });
            //FUNCIONA, con esto agrego un cierto filtro para no traer todoooo. PROBADO
        }


        [HttpGet("ObtenerUsuariosNombre")]
        public async Task<IActionResult> ObtenerUsuariosPorNombre(string nombre)
        {
            var lista = await _administradorServicio.ObtenerUsuariosPorNombre(nombre);

            if (lista.IsNullOrEmpty())
                return NotFound("Usuario no encontrado.");
            return Ok(new { Usuarios = lista });
            //FUNCIONA, con esto agrego un cierto filtro para no traer todoooo. PROBADO
        }

        [HttpGet("ObtenerUsuarioId")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            var user = await _administradorServicio.ObtenerUsuarioPorId(id);

            return user != null ? Ok(user): NotFound("Usuario no encontrado");
            //FUNCIONA. PROBADO
        }

        [HttpPost("RegistroUsuario")]
        public async Task<IActionResult> RegistrarUsuario(CrudUsuarioDTO usuario)
        {   
            //Validacion de modelos con DataAnotation
            if (await _administradorServicio.RegistrarUsuario(usuario))
            {   //Validaciones basicas
                if (usuario == null) return BadRequest("Usuario no puede ser null");
                //Si un modelo no es valido, valida estado del formulario, si alguna validacion no se cumple
                if (!ModelState.IsValid) return BadRequest(ModelState);
                return Ok("Registro exitoso");
            }
            return BadRequest("Registro fallido. Revisar informacion");
            //PROBADO, funciona
        }

        [HttpPut("ActualizarUsuarioPorId")]
        public async Task<IActionResult> ActualizarUsuarioPorID(CrudUsuarioDTO usuario)
        {
            return Ok(await _administradorServicio.ActualizarUsuarioPorID(usuario));
        }

        [HttpDelete("BorrarUsuarioPorId")]
        public async Task<IActionResult> BorrarUsuarioPorID(int id)
        {
            return Ok(await _administradorServicio.BorrarUsuarioPorID(id));
        }

        [HttpGet("VerTorneos")]
        public async Task<IActionResult> VerTorneosYPartidas()
        {
            return Ok(await _administradorServicio.VerTorneosYpartidas());
            //Funciona con Exito?, ver paginacion
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
