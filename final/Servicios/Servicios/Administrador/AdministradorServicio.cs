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


namespace Servicios.Servicios.Administrador
{
    public class AdministradorServicio(IDAOAdministrador daoAdministrador, IComunes common) : IAdministradorServicio
    {

        private readonly IDAOAdministrador _daoAdministrador = daoAdministrador;

        private readonly IComunes _common = common;

        //METODOS
        public async Task<List<UsuarioDTO>> ObtenerUsuariosPorNombre(string patron)
        {
            return await _daoAdministrador.ObtenerUsuariosPorNombre(patron);

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

        public async Task<bool> BorrarUsuarioPorID(CrudUsuarioDTO usuario)
        {
            return await _daoAdministrador.BorrarUsuarioPorID(usuario);

        }
        public async Task<List<TorneoDTO>> VerTorneosYpartidas()
        {
            return await _daoAdministrador.VerTorneosYPartidas();
        }

        public async Task<string> CancelarTorneos(int torneoid, string texto)
        {
            if(await _daoAdministrador.CancelarTorneos(torneoid, texto))
            {
                return "Torneo cancelado con exito";
            }
            return "No fue posible cancelar";
           
        }




    }
}
