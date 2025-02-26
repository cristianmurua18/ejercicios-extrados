using AccesoDatos.DAOs.Organizador;
using Entidades.DTOs;
using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades.Utilidades;

namespace Servicios.Servicios.Organizador
{
    public class OrganizadorServicio(IDAOOrganizador daoOrganizador, IComunes comunes, IHttpContextAccessor httpContextAccessor) : IOrganizadorServicio
    {
        //Es para traer la info de los claims del usuario logeado
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private readonly IDAOOrganizador _daoOrganizador = daoOrganizador;

        private readonly IComunes _common = comunes;

        public async Task<List<Usuario>> VerListadoUsuarios(string rol)
        {
            return await _daoOrganizador.VerListadoUsuarios(rol);
        }

        public async Task<bool> RegistrarJuez(CrudUsuarioDTO usuario)
        {   //Encripto contraseña
            usuario.Contraseña = _common.EncriptarSHA256(usuario.Contraseña!);
            return await _daoOrganizador.RegistrarJuez(usuario);
        }
        public async Task<bool> CrearTorneo(CrudTorneoDTO torneo)
        {
            return await _daoOrganizador.CrearTorneo(torneo);
        }

        public async Task<bool> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie)
        {
            return await _daoOrganizador.CrearTorneoSerieHabilitada(serie);
        }

        public async Task<bool> EditarTorneo(CrudTorneoDTO torneo)
        {
            return await _daoOrganizador.EditarTorneo(torneo);
        }

        public async Task<bool> CancelarTorneo(int idtorneo, string estado)
        {
            return await _daoOrganizador.CancelarTorneo(idtorneo, estado);
        }

        public async Task<bool> CrearRondas(int idTorneo)
        {
            //IDEAS, contar cantidad de Inscriptos, en base a eso calcular cantidad de rondas

            //Traigo el id del usuario al autenticarse, el organizador
            var organizador = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirst("UsuarioID")?.Value);


            //Veo en que torneo quiero crear las partidas, que yo organice si o si
            var torneo = await _daoOrganizador.TraerTorneo(organizador, idTorneo);

            if (torneo != null)
            {
                var partidas = (torneo.PartidasDiarias * torneo.DiasDeDuracion) - 1;

                var inscriptos = await _daoOrganizador.ContarInscriptosByTorneo(idTorneo);

                var ronda = new RondaDTO();

                if (inscriptos % 2 == 0)
                {
                    ronda.RondaID = 1;
                    ronda.CantidadPartidas = partidas;
                    ronda.IdTorneo = idTorneo;
                    ronda.JugadoresPar = true;
                }
                else               //SEGUIR REVISANDO
                {
                    ronda.RondaID = 1;
                    ronda.CantidadPartidas = partidas;
                    ronda.IdTorneo = idTorneo;
                    ronda.JugadoresPar = false;

                }

                //Usar un bucle por cada ronda, ir creando las partidas - Asignando dias y horarios

                var nueva = await _daoOrganizador.CrearRondas(ronda);


                var part = new PartidaDTO();

                part.PartidaID = 1;
                part.IdRonda = 1;
                part.FyHInicioP = torneo.FyHInicioT;

                for (int i = 1; i <= ronda.CantidadPartidas; i++)
                {
                    await _daoOrganizador.CrearPartida(part);

                    part.PartidaID++;
                    part.FyHFinP.AddMinutes(30);
                }

                return true;
            }
            else
                throw new InvalidOperationException("Registro fallido. No fue posible ubicar el torneo.Revise informacion.");

            
        }


        public async Task<bool> ModificarPartida(PartidaDTO partida)
        {
            return await _daoOrganizador.ModificarPartida(partida);
        }
    }
}
