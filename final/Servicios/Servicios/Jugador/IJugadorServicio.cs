﻿using Entidades.DTOs.Cruds;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.Servicios.Jugador
{
    public interface IJugadorServicio
    {
        public Task<int> CrearColeccion(int idCarta);
        public Task<int> CrearMazo(string nombreMazo);
        public Task<string> VerMisMazos();
        public Task<bool> RegistrarCartas(CrudMazoCartasDTO cartas, int idTorneo);
        public Task<bool> RegistroEnTorneo(int idTorneo, int idMazo);
    }
}
