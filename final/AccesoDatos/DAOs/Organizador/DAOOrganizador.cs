using Entidades.DTOs;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Modelos;
using Entidades.DTOs.Cruds;
using System.Collections;
using Entidades.DTOs.Respuestas;
using Utilidades.Utilidades;
using Microsoft.IdentityModel.Tokens;

namespace AccesoDatos.DAOs.Organizador
{
    public class DAOOrganizador(IDbConnection dbConnection, IComunes comunes) : IDAOOrganizador
    {

        private readonly IDbConnection _dbConnection = dbConnection;

        private readonly IComunes _comunes = comunes;


        #region Uso general de Organizadores

        public async Task<List<TorneoJugadorDTO>> VerInscriptosByTorneo(int idTorneo)
        {
            var sql = "SELECT * FROM TorneoJugadores WHERE IdTorneo=idTorneo;";

            var inscriptos = await _dbConnection.QueryAsync<TorneoJugadorDTO>(sql, new { idTorneo });

            return inscriptos.ToList();

        }

        public async Task<bool> EliminarInscriptoByTorneo(int idJugador)
        {                                                                   //Agregar condicion Aceptada=0
            var sql = "UPDATE TorneoJugadores SET Aceptada=0 WHERE IdJugador=idJugador;";

            var filas = await _dbConnection.ExecuteAsync(sql, new { idJugador });

            return filas > 0;

        }

        public async Task<int> ContarInscriptosByTorneo(int idTorneo)
        {
            var sql = "SELECT COUNT(IdJugador) FROM TorneoJugadores WHERE IdTorneo=idTorneo AND Aceptada=1;";

            var cantidad = await _dbConnection.ExecuteScalarAsync<int>(sql, new { idTorneo });

            return cantidad;

        }

        public async Task<int> ContarInscriptosByTorneo(int idTorneo, IDbTransaction transaction)
        {
            var sql = "SELECT COUNT(*) FROM TorneoJugadores WHERE IdTorneo=idTorneo AND Aceptada=1;";

            var cantidad = await _dbConnection.ExecuteScalarAsync<int>(sql, new { idTorneo }, transaction);

            return cantidad;

        }

        public async Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo)
        {
            var sqlSelect = @"SELECT * from Torneos WHERE Organizador=@organizador AND TorneoID=@idTorneo;";

            var torneo = await _dbConnection.QueryFirstOrDefaultAsync<TorneoDTO>(sqlSelect, new { organizador, idTorneo });

            return torneo!;

        }

        public async Task<TorneoDTO> TraerTorneo(int organizador, int idTorneo, IDbTransaction transaction)
        {
            var sqlSelect = @"SELECT * from Torneos WHERE Organizador=@organizador AND TorneoID=@idTorneo;";

            var torneo = await _dbConnection.QueryFirstOrDefaultAsync<TorneoDTO>(sqlSelect, new { organizador, idTorneo }, transaction);

            return torneo!;

        }

        public async Task<UsuarioDTO> VerUsuario(int id)
        {                                           
            var sqlSelect = @"SELECT * 
                            FROM usuarios 
                            WHERE UsuarioID=@id;";

            var usuario = await _dbConnection.QueryFirstOrDefaultAsync<UsuarioDTO>(sqlSelect, new { id });

            return usuario;

        }

        public async Task<List<UsuarioPaisDTO>> VerListadoUsuarios(string rol, int miUsuario)
        {                                           //ver de manejar estado 1 o 0
            var sqlSelect = @"SELECT * 
                            FROM usuarios us
                            INNER JOIN Paises pa ON us.IdPaisOrigen = pa.PaisID
                            WHERE (us.CreadoPor = @miUsuario OR us.CreadoPor IS NULL) 
                            AND us.Rol = @rol;";

            var listado = await _dbConnection.QueryAsync<UsuarioPaisDTO>(sqlSelect, new { rol, miUsuario });

            return listado.ToList();

        }

        public async Task<bool> RegistrarJuez(InsertarJuezDTO juez, int miUsuario)
        {
            var sqlInsert = @"INSERT
                INTO Usuarios(NombreApellido,Alias,IdPaisOrigen,Email,NombreUsuario,Contraseña,Rol,CreadoPor,Activo) 
                VALUES(@NombreApellido,@Alias,@IdPaisOrigen,@Email,@NombreUsuario,@Contraseña,@Rol,@CreadoPor,@Activo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, new
            {
                juez.NombreApellido,
                juez.Alias,
                juez.IdPaisOrigen,
                juez.Email,
                juez.NombreUsuario,
                juez.Contraseña,
                juez.Rol,
                miUsuario,
                juez.Activo
            });

            return result > 0;

        }

        public async Task<bool> AsignarJuezATorneo(int idJuez, int idTorneo)
        {
            var sqlInsert = @"INSERT
                INTO TorneoJueces 
                VALUES(@IdJuez,@IdTorneo);";

            var result = await _dbConnection.ExecuteAsync(sqlInsert, new { idJuez, idTorneo});

            return result > 0;

        }

        public async Task<bool> CrearTorneo(InsertarTorneoDTO torneo, int miUsuario)
        {
            var sqlInsert = @"INSERT INTO Torneos(NombreTorneo, FyHInicioT,FyHFinT,Estado,IdPaisRealizacion,JugadorGanador,Organizador, PartidasDiarias, DiasDeDuracion,MaxJugadores) 
                                     VALUES(@NombreTorneo,@FyHInicioT,@FyHFinT,@Estado,@IdPaisRealizacion,@JugadorGanador,@Organizador,@PartidasDiarias,@DiasDeDuracion,@MaxJugadores);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, new { 
            torneo.NombreTorneo,
            torneo.FyHInicioT,
            torneo.FyHFinT,
            torneo.Estado,
            torneo.IdPaisRealizacion,
            torneo.JugadorGanador,
            Organizador = miUsuario,
            torneo.PartidasDiarias,
            torneo.DiasDeDuracion,
            torneo.MaxJugadores
            });

            //Agrega un torneo con Exito?
            return resultado > 0;
        }

        public async Task<bool> CrearTorneoSerieHabilitada(CrudTorneoSerieHabilitadaDTO serie)
        {
            var sqlInsert = @"INSERT INTO TorneoSerieHabilitada(IdTorneo,IdSerie) 
                                     VALUES(@IdTorneo,@IdSerie);";

            var resultado = await _dbConnection.ExecuteAsync(sqlInsert, new { serie.IdTorneo, serie.IdSerie });

            //Agrega un torneo con Exito?
            return resultado > 0;
        }

        public async Task<bool> EditarTorneo(CrudTorneoDTO torneo)
        {
            var sqlUpdate =
                @"UPDATE Torneos 
                SET FyHInicioT=@FyHInicioT, FyHFinT=@FyHFinT, Estado=@Estado, IdPaisRealizacion=@IdPaisRealizacion, PartidaActual=@PartidaActual, 
                JugadorGanador=@JugadorGanador, Organizador=@Organizador, PartidasDiarias=@PartidasDiarias,DiasDeDuracion=@DiasDeDuracion
                WHERE TorneoID=@TorneoID;";

            var resultado = await _dbConnection.ExecuteAsync(sqlUpdate, torneo);

            return resultado > 0;

        }

        public async Task<bool> CancelarTorneo(int idtorneo, string estado)
        {
            var sqUpdate =
                @"UPDATE Torneos 
                SET Estado=@estado 
                WHERE TorneoID=@idtorneo;";
            //Pongo directamente cancelado?
            var resultado = await _dbConnection.ExecuteAsync(sqUpdate, new { idtorneo });

            return resultado > 0;

        }

        public async Task<bool> CerrarInscrpcionTorneo(int idTorneo)
        {
            var sqUpdate =
                @"UPDATE Torneos 
                SET Estado='Partidas' 
                WHERE TorneoID=@idtorneo;";
            
            var resultado = await _dbConnection.ExecuteAsync(sqUpdate, new { idTorneo });

            return resultado > 0;
        }

        public async Task<bool> GenerarRondasYPartidas(int organizador, int idTorneo)
        {
            using var tran = _dbConnection.BeginTransaction();

            //Traigo el torneo organizado por mi
            var torneo = await TraerTorneo(organizador, idTorneo, tran);
            
            //Traigo los jugadores inscriptos, un listado de sus ids
            var sqlInscriptos = "SELECT IdJugador from TorneoJugadores where IdTorneo=@idTorneo";

            var verInscriptos = await _dbConnection.QueryAsync<TorneoJugadorDTO>(sqlInscriptos, new { IdTorneo = idTorneo }, tran);

            // Validar bit a bit que la cantidad de jugadores sea potencia de 2
            if (!_comunes.EsPotenciaDeDos(verInscriptos.Count()))
                throw new InvalidOperationException("La cantidad de jugadores debe ser una potencia de 2. Revise inscriptos. Operacion cancelada.");



            //los pongo en una pila, para luego asignarlos en la creacion de partidas
            var pilaJugadores = new Stack<int>(verInscriptos.Select(jug => jug.IdJugador));

            //Valido que exista el torneo y seas el organizador, y que esten en estado partidas. Con las incripciones cerradas
            if (torneo!=null && (torneo.Estado == "Partidas"))
            {
                int cantidadRondas = (int)Math.Log2(verInscriptos.Count());
                int partidasEnRonda = verInscriptos.Count() / 2;

                int? jugadorUno = null;
                int? jugadorDos = null;

                // Insertar rondas
                for (int i = 1; i <= cantidadRondas; i++)
                {
                    var idRonda = await _dbConnection.ExecuteScalarAsync<int>(
                        @"INSERT INTO Rondas 
                        OUTPUT INSERTED.RondaID
                        VALUES (@IdTorneo, @NumeroRonda);",
                        new { IdTorneo = idTorneo, NumeroRonda = i}, tran);

                    if (!(idRonda > 0))
                    {
                        tran.Rollback();
                        return false;
                    }

                    //Insertar partidas
                    for (int j = 1; j <= partidasEnRonda; j++)
                    {
                        if(i == 1 && pilaJugadores.Count != 0) //Esto es todas las partidas que se creen en la primera ronda?
                        {   //ESTO NO FUNCIONA. Mete los ultimos dos jugadores en el resto de las rondas
                            jugadorUno = pilaJugadores.Pop();
                            jugadorDos = pilaJugadores.Pop();
                        }

                        await _dbConnection.ExecuteAsync(
                            @"INSERT INTO Partidas (IdRonda, NumeroPartida, FyHInicioP, JugadorUno, JugadorDos)
                            VALUES (@IdRonda, @NumeroPartida, @FyHInicioP, @JugadorUno, @JugadorDos);",
                            new { IdRonda = idRonda, NumeroPartida=j, FyHInicioP = torneo.FyHInicioT, JugadorUno = jugadorUno, JugadorDos = jugadorDos }, tran);

                        torneo.FyHInicioT = torneo.FyHInicioT.AddMinutes(30);
                        //Aca la idea del torneo es jugar hasta las 18hs del dia, asi que debo controlar que si la hora es igual a las 17.30, debo agregar un dia y comenzar a las 10hs
                        if (torneo.FyHInicioT.Hour == 17 && torneo.FyHInicioT.Minute >= 30)
                        {
                            torneo.FyHInicioT = torneo.FyHInicioT.Date.AddDays(1).AddHours(10);
                        }


                    }
                    partidasEnRonda /= 2; // La siguiente ronda tiene la mitad de partidas
                }
                tran.Commit();
                return true;
            }
            return false;
            throw new ArgumentException("No existe el torneo o tu no lo organizas, luego debes cerrar inscripciones. Operacion cancelada.");
        }

        //Crear metodo para ver Rondas y partidas
        public async Task<List<PartidaRondaDTO>> VerRondasYPartidas(int idTorneo)
        {
            var sqlSelect =
                @"SELECT * FROM Partidas par
                INNER JOIN Rondas ron
                ON par.IdRonda = ron.RondaID
                WHERE ron.IdTorneo=@IdTorneo;";

            var listadoRonda = await _dbConnection.QueryAsync<PartidaRondaDTO>(sqlSelect, new { idTorneo });

            return listadoRonda.ToList();

        }

        public async Task<List<PartidaRondaDTO>> VerRondasYPartidas(int idTorneo, int idRonda, IDbTransaction transaction)
        {
            var sqlSelect =
                @"SELECT * FROM Partidas par
                INNER JOIN Rondas ron
                ON par.IdRonda = ron.RondaID
                WHERE ron.IdTorneo=@IdTorneo AND ron.RondaID=@IdRonda AND par.IdRonda=@IdRonda;";

            var listadoRonda = await _dbConnection.QueryAsync<PartidaRondaDTO>(sqlSelect, new { idTorneo, idRonda }, transaction);

            return listadoRonda.ToList();

        }

        public async Task<bool> AvanzarRonda(int idTorneo, int idRonda)
        {
            using var tran = _dbConnection.BeginTransaction();
            //Revisar 
            var partidasRondaActual = await VerRondasYPartidas(idTorneo, idRonda, tran);

            if(partidasRondaActual.IsNullOrEmpty() || partidasRondaActual.Count == 0)
                throw new InvalidOperationException("No hay partidas disponibles para la ronda actual. Operacion cancelada.");
            //Apilo los ganadores de la ronda
            var pilaJugadores = new Stack<int>(partidasRondaActual.Select(jug => jug.Ganador));
            
            var idRondaProx = idRonda + 1;

            var partidasRondaSig = await VerRondasYPartidas(idTorneo, idRondaProx, tran);
            if (partidasRondaSig.IsNullOrEmpty() || partidasRondaSig.Count == 0)
                throw new InvalidOperationException("No hay partidas disponibles para la ronda siguiente. Operacion cancelada.");

            foreach (var partida in partidasRondaSig)
            {  //Saco los jugadores ganadores de la pila
                var jugadorUno = pilaJugadores.Pop();
                var jugadorDos = pilaJugadores.Pop();

                var sqlUpdate =
                @"UPDATE Partidas SET JugadorUno=@jugadorUno, JugadorDos=@jugadorDos
                where PartidaID=@partida.PartidaID;";

                var resultado = await _dbConnection.ExecuteAsync(sqlUpdate, new { jugadorUno, jugadorDos, partida.PartidaID }, tran);
                //Ver cuantas filas modifica
                if (resultado <= 0)
                {
                    tran.Rollback();
                    return false;

                }
                   
            }
            tran.Commit();
            return true;
        
        }






        #endregion
    }
}
