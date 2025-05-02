using Entidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;
using Utilidades.Utilidades;
using Entidades.DTOs.Cruds;
using AccesoDatos.DAOs.Administrador;
using Entidades.DTOs.Varios;


namespace Servicios.Servicios.Administrador
{
    public class AdministradorServicio(IDAOAdministrador daoAdministrador, IComunes common) : IAdministradorServicio
    {

        private readonly IDAOAdministrador _daoAdministrador = daoAdministrador;

        private readonly IComunes _common = common;

        //METODOS

        public async Task<List<UsuarioDTO>> ObtenerUsuariosPorRol(string rol)
        {
            return await _daoAdministrador.ObtenerUsuariosPorRol(rol);
        }

        public async Task<List<UsuarioDTO>> ObtenerUsuariosPorNombre(string nombre)
        {
            return await _daoAdministrador.ObtenerUsuariosPorNombre(nombre);
        }

        public async Task<UsuarioDTO> ObtenerUsuarioPorId(int id)
        {
            return await _daoAdministrador.ObtenerUsuarioPorId(id);
        }

        public async Task<bool> RegistrarUsuario(CrudUsuarioDTO usuario)
        {
            //Encriptar contraseña al guardar usuario
            usuario.Contraseña = _common.EncriptarSHA256(usuario.Contraseña!);
            return await _daoAdministrador.RegistrarUsuario(usuario);

        }
            
        public async Task<bool> ActualizarUsuarioPorID(CrudUsuarioDTO usuario)
        {
            return await _daoAdministrador.ActualizarUsuarioPorID(usuario);
        }

        public async Task<bool> BorrarUsuarioPorID(int id)
        {
            return await _daoAdministrador.BorrarUsuarioPorID(id);
        }
        public async Task<List<TorneoDTO>> VerTorneos()
        {
            return await _daoAdministrador.VerTorneos();
        }

        public async Task<bool> CancelarTorneo(int torneoid)
        {
            return await _daoAdministrador.CancelarTorneo(torneoid);
        }


    }
}
