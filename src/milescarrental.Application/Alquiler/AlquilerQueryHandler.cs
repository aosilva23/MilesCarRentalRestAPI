using Dapper;
using MediatR;
using Oracle.ManagedDataAccess.Client;
using milescarrental.Application.Configuration.Data;
using milescarrental.Application.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace milescarrental.Application.Alquiler
{
    public class AlquilerQueryHandler : IRequestHandler<AlquilerQuery, List<AlquilerDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        string mensajeAdvertencia = "";
        List<AlquilerDTO> listAlquilerAdvertencia = new List<AlquilerDTO>();

        public AlquilerQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<AlquilerDTO>> Handle(AlquilerQuery request, CancellationToken cancellationToken)
        {
            List<AlquilerDTO> listAlquiler = new List<AlquilerDTO>();
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            if(mensajeAdvertencia == "")
            {
                try
                {
                    OracleCommand objCmd = new OracleCommand();
                    objCmd.Connection = (OracleConnection)connection;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_MCR_PROCES_ALQUILER";

                    objCmd.Parameters.Add("ALQID", OracleDbType.Int32).Value = request.alquiler.id;
                    objCmd.Parameters.Add("ALQIDCLIENTE", OracleDbType.Int32).Value = request.alquiler.idCliente;
                    objCmd.Parameters.Add("ALQIDVEHICULO", OracleDbType.Int32).Value = request.alquiler.idVehiculo;
                    objCmd.Parameters.Add("ALQFECHAINICIO", OracleDbType.Varchar2).Value = request.alquiler.fechaInicio;
                    objCmd.Parameters.Add("ALQFECHAFIN", OracleDbType.Varchar2).Value = request.alquiler.fechaFin;
                    objCmd.Parameters.Add("ALQKILOMETRAJEINICIAL", OracleDbType.Int32).Value = request.alquiler.kilometrajeInicial;
                    objCmd.Parameters.Add("ALQKILOMETRAJEFINAL", OracleDbType.Int32).Value = request.alquiler.kilometrajeFinal;
                    objCmd.Parameters.Add("ALQLOCLATITUDVEHICULO", OracleDbType.Varchar2).Value = request.alquiler.locLatitudVehiculo;
                    objCmd.Parameters.Add("ALQLOCLONGITUDVEHICULO", OracleDbType.Varchar2).Value = request.alquiler.locLongitudVehiculo;
                    objCmd.Parameters.Add("ALQLOCLATITUDCLIENTE", OracleDbType.Varchar2).Value = request.alquiler.locLatitudCliente;
                    objCmd.Parameters.Add("LOCLONGITUDCLIENTE", OracleDbType.Varchar2).Value = request.alquiler.locLongitudCliente;
                    objCmd.Parameters.Add("ALQCOSTO", OracleDbType.Int32).Value = request.alquiler.costoAlquiler;
                    objCmd.Parameters.Add("ALQESTADO", OracleDbType.Varchar2).Value = request.alquiler.estado;
                    objCmd.Parameters.Add("ALQOBSERVACIONES", OracleDbType.Varchar2).Value = request.alquiler.observaciones;
                    objCmd.Parameters.Add("ALQUSUARIOCREACION", OracleDbType.Varchar2).Value = request.alquiler.usuario;
                    objCmd.Parameters.Add("PROCESO", OracleDbType.Int32).Value = request.alquiler.proceso;

                    OracleDataReader reader = objCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        AlquilerDTO alquiler = new AlquilerDTO();

                        alquiler.id = (Int64)reader["ALQID"];
                        alquiler.idCliente = (Int64)reader["ALQIDCLIENTE"];
                        alquiler.idVehiculo = (Int64)reader["ALQIDVEHICULO"];
                        alquiler.fechaInicio = DBNull.Value.Equals(reader["ALQFECHAINICIO"]) ? DateTime.Now.ToString() : ((DateTime)reader["ALQFECHAINICIO"]).ToString();
                        alquiler.fechaFin = DBNull.Value.Equals(reader["ALQFECHAFIN"]) ? DateTime.Now.ToString() : ((DateTime)reader["ALQFECHAFIN"]).ToString();
                        alquiler.kilometrajeInicial = (Int32)reader["ALQKILOMETRAJEINICIAL"];
                        alquiler.kilometrajeFinal = (Int32)reader["ALQKILOMETRAJEFINAL"];
                        alquiler.locLatitudVehiculo = (string)reader["ALQLOCLATITUDVEHICULO"];
                        alquiler.locLongitudVehiculo = (string)reader["ALQLOCLONGITUDVEHICULO"];
                        alquiler.locLatitudCliente = (string)reader["ALQLOCLATITUDCLIENTE"];
                        alquiler.locLongitudCliente = (string)reader["ALQLOCLONGITUDCLIENTE"];
                        alquiler.costoAlquiler = (Int32)reader["ALQCOSTO"];
                        alquiler.estado = (string)reader["ALQESTADO"].ToString();
                        alquiler.observaciones = (string)reader["ALQOBSERVACIONES"].ToString();
                        alquiler.fechaRegistro = DBNull.Value.Equals(reader["ALQFECHAREGISTRO"]) ? DateTime.Now.ToString() : ((DateTime)reader["ALQFECHAREGISTRO"]).ToString();
                        alquiler.proceso = (decimal)reader["PROCESO"];
                        alquiler.usuario = (string)reader["ALQUSUARIOCREACION"];
                        alquiler.mensaje = (string)reader["MENSAJE"].ToString();
                        listAlquiler.Add(alquiler);
                    }

                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.ToString();

                    AlquilerDTO alquiler = new AlquilerDTO();

                    alquiler.id = 0;
                    alquiler.idCliente = 0;
                    alquiler.idVehiculo = 0;
                    alquiler.fechaInicio = "";
                    alquiler.fechaFin = "";
                    alquiler.kilometrajeInicial = 0;
                    alquiler.kilometrajeFinal = 0;
                    alquiler.locLatitudVehiculo = "";
                    alquiler.locLongitudVehiculo = "";
                    alquiler.locLatitudCliente = "";
                    alquiler.locLongitudCliente = "";
                    alquiler.costoAlquiler = 0;
                    alquiler.estado = "";
                    alquiler.observaciones = "";
                    alquiler.fechaRegistro = "";
                    alquiler.mensaje = "Error en BD: " + mensaje.Substring(0, 200);

                    listAlquilerAdvertencia.Add(alquiler);

                    return listAlquilerAdvertencia;
                }

                return listAlquiler;
            }
            else
            {
                AlquilerDTO alquiler = new AlquilerDTO();

                alquiler.id = 0;
                alquiler.idCliente = 0;
                alquiler.idVehiculo = 0;
                alquiler.fechaInicio = "";
                alquiler.fechaFin = "";
                alquiler.kilometrajeInicial = 0;
                alquiler.kilometrajeFinal = 0;
                alquiler.locLatitudVehiculo = "";
                alquiler.locLongitudVehiculo = "";
                alquiler.locLatitudCliente = "";
                alquiler.locLongitudCliente = "";
                alquiler.costoAlquiler = 0;
                alquiler.estado = "";
                alquiler.observaciones = "";
                alquiler.fechaRegistro = "";
                alquiler.mensaje = "Advertencia: " + mensajeAdvertencia;

                listAlquilerAdvertencia.Add(alquiler);

                return listAlquilerAdvertencia;
            }            
        }

        public Boolean EsFecha(String fecha)
        {
            try
            {
                DateTime.Parse(fecha);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string validarTamanoParametros(AlquilerQuery request)
        {
            // Validar tamaño de parametros:
            if (request.alquiler.estado.Length > 16) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro estado no puede ser mayora a 16. "; }
            if (request.alquiler.fechaRegistro.Length > 10) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro fecha no puede ser mayora a 10, y debe cumplir con el formato DD/MM/YYYY. "; }

            return mensajeAdvertencia;
        }


    }
}



