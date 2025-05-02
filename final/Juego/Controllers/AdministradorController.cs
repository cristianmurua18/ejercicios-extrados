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
using Validaciones.Excepciones;
using Entidades.Modelos;

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
                throw new ItemNotFoundException($"Usuarios con Rol: {rol} no encontrados");
            return Ok(new { Usuarios = lista });
            //FUNCIONA, con esto agrego un cierto filtro para no traer todoooo. PROBADO
        }


        [HttpGet("ObtenerUsuariosNombre")]
        public async Task<IActionResult> ObtenerUsuariosPorNombre(string nombre)
        {
            var lista = await _administradorServicio.ObtenerUsuariosPorNombre(nombre);

            if (lista.IsNullOrEmpty())
                throw new ItemNotFoundException($"Usuario {nombre} no encontrados.");
            return Ok(new { Usuarios = lista });
            //FUNCIONA, con esto agrego un cierto filtro para no traer todoooo. PROBADO
        }

        [HttpGet("ObtenerUsuarioId")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            var user = await _administradorServicio.ObtenerUsuarioPorId(id);

            return user != null ? Ok(user): throw new ItemNotFoundException($"Usuario con Id: {id} no encontrado");
            //FUNCIONA. PROBADO
        }

        [HttpPost("RegistroUsuario")]
        public async Task<IActionResult> RegistrarUsuario(CrudUsuarioDTO usuario)
        {   
            //Validacion de modelos con DataAnotation
            if (await _administradorServicio.RegistrarUsuario(usuario))
            {   //Validaciones basicas
                if (usuario == null) throw new ItemNotFoundException("Usuario no puede ser null");
                //Si un modelo no es valido, valida estado del formulario, si alguna validacion no se cumple
                if (!ModelState.IsValid) throw new InvalidRequestException($"El modelo no esta bien ingresado. {ModelState}");
                return Ok("Registro exitoso");
            }
            throw new InvalidRequestException("Registro fallido. Revisar informacion");
            //PROBADO, funciona
        }

        [HttpPut("ActualizarUsuarioPorId")]
        public async Task<IActionResult> ActualizarUsuarioPorID(CrudUsuarioDTO usuario)
        {
            var res = await _administradorServicio.ActualizarUsuarioPorID(usuario);

            if(res)
                Ok($"Usuario Id: {usuario.UsuarioID}, actualizado con exito");
            throw new InvalidRequestException($"No fue posible actualizar el usuario id:{usuario.UsuarioID}");
        }

        [HttpDelete("BorrarUsuarioPorId")]
        public async Task<IActionResult> BorrarUsuarioPorID(int id)
        {
            var res = await _administradorServicio.BorrarUsuarioPorID(id);

            if (res)
                Ok($"Usuario Id: {id}, borrado con exito");
            throw new InvalidRequestException($"No fue posible borrar el usuario id:{id}");
 
        }

        [HttpGet("VerTorneos")]
        public async Task<IActionResult> VerTorneos()
        {
            var resp = await _administradorServicio.VerTorneos();
                
            if (resp.IsNullOrEmpty() || resp.Count == 0)
                throw new ItemNotFoundException("Lista vacia");
            return Ok(resp);
            //Funciona con Exito?, ver paginacion
        }

        //Metodo para 

        [HttpPost("CancelarTorneo")]
        public async Task<IActionResult> CancelarTorneo(int torneoid)
        {
            var resp = await _administradorServicio.CancelarTorneo(torneoid);

            if(resp)
                 return Ok($"Torneo con Id: {torneoid} cancelado con exito");
            throw new InvalidRequestException($"No fue posible cancelar el Torneo Id: {torneoid}");
            
        }

        #endregion Final Administradores




    }
}
