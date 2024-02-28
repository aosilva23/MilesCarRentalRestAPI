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

namespace milescarrental.Application.Vehiculo
{
    public class VehiculoQueryHandler : IRequestHandler<VehiculoQuery, List<VehiculoDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        string mensajeAdvertencia = "";
        List<VehiculoDTO> listVehiculoAdvertencia = new List<VehiculoDTO>();

        public VehiculoQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<VehiculoDTO>> Handle(VehiculoQuery request, CancellationToken cancellationToken)
        {
            List<VehiculoDTO> listVehiculo = new List<VehiculoDTO>();
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            #region Preparacion de Datos segun sea el Proceso:            

            // Procesos Vehiculo -- > Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar i Inactivar(4), Listar todos(5), Busqueda Generica(6)
            if (request.vehiculo.proceso < 1 || request.vehiculo.proceso > 6)
            {
                mensajeAdvertencia = "El parametro proceso debe estar entre 1 y 6. ";
            }

            // BUSQUEDA EXACTA
            if (request.vehiculo.proceso == 1)
            {
                // Validar parametros obligatorios:
                if (request.vehiculo.placa == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el placa del vehiculo. "; }
                if (request.vehiculo.modelo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el modelo del vehiculo. "; }
                if (request.vehiculo.kilometraje < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar un valor de kilometraje correcto. "; }
                if (request.vehiculo.tipo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el modelo el tipo (NUEVO, USADO). "; }
                if (request.vehiculo.nomeclaturamotor == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar la nomeclatura del vehiculo. "; }
                if (request.vehiculo.estado == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado del vehiculo (MANTENIMIENTO, ALQUILADO, DISPONIBLE, RETIRADO). "; }
                if (request.vehiculo.usuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el usario que hace el registro. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.vehiculo.mensaje = "";
                    request.vehiculo.fechaRegistro = "01/01/1999";
                }
            }

            // INSERTAR
            if (request.vehiculo.proceso == 2)
            {
                // Validar parametros obligatorios:
                if (request.vehiculo.placa == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el placa del vehiculo. "; }
                if (request.vehiculo.modelo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el modelo del vehiculo. "; }
                if (request.vehiculo.kilometraje < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar un valor de kilometraje correcto. "; }
                if (request.vehiculo.tipo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el modelo el tipo (NUEVO, USADO). "; }
                if (request.vehiculo.nomeclaturamotor == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar la nomeclatura del vehiculo. "; }
                if (request.vehiculo.estado == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado del vehiculo (MANTENIMIENTO, ALQUILADO, DISPONIBLE, RETIRADO). "; }
                if (request.vehiculo.usuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el usario que hace el registro. "; }
                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.vehiculo.mensaje = "";
                    request.vehiculo.fechaRegistro = "01/01/1999";
                }
            }

            // ACTUALIZAR
            if (request.vehiculo.proceso == 3)
            {
                // Validar parametros obligatorios:
                if (request.vehiculo.placa == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el placa del vehiculo. "; }
                if (request.vehiculo.modelo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el modelo del vehiculo. "; }
                if (request.vehiculo.kilometraje < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar un valor de kilometraje correcto. "; }
                if (request.vehiculo.tipo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el modelo el tipo (NUEVO, USADO). "; }
                if (request.vehiculo.nomeclaturamotor == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar la nomeclatura del vehiculo. "; }
                if (request.vehiculo.estado == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado del vehiculo (MANTENIMIENTO, ALQUILADO, DISPONIBLE, RETIRADO). "; }
                if (request.vehiculo.usuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el usario que hace el registro. "; }

                // Valores por defecto:
                if (request.vehiculo.fechaRegistro == "") { request.vehiculo.fechaRegistro = "01/01/1999"; }

                // Validar tamaño de parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (!this.EsFecha(request.vehiculo.fechaRegistro)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
                if (!(request.vehiculo.tipo == "NUEVO" || request.vehiculo.tipo == "USADO")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro tipo incorrecto (NUEVO, USADO) "; }
                if (!(request.vehiculo.estado == "MANTENIMIENTO" || request.vehiculo.estado == "ALQUILADO" || request.vehiculo.estado == "DISPONIBLE" || request.vehiculo.estado == "RETIRADO")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro tipo incorrecto (NUEVO, USADO) "; }
            }

            // ELIMINAR O INACTIVAR
            if (request.vehiculo.proceso == 4)
            {
                // Validar parametros obligatorios:
                if (request.vehiculo.id < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el id. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.vehiculo.placa = "X";
                    request.vehiculo.modelo = "X";
                    request.vehiculo.kilometraje = 0;
                    request.vehiculo.tipo = "X";
                    request.vehiculo.nomeclaturamotor = "X";
                    request.vehiculo.estado = "X";
                    request.vehiculo.fechaRegistro = "01/01/1999";
                }
            }

            // LISTAR TODOS
            if (request.vehiculo.proceso == 5)
            {
                // Valores por defecto:
                request.vehiculo.id = 0;
                request.vehiculo.modelo = "X";
                request.vehiculo.kilometraje = 0;
                request.vehiculo.tipo = "X";
                request.vehiculo.nomeclaturamotor = "X";
                request.vehiculo.estado = "X";
                request.vehiculo.fechaRegistro = "01/01/1999";
            }

            // BUSQUEDA GENERICA
            if (request.vehiculo.proceso == 6)
            {
                // Valores por defecto:
                if (request.vehiculo.modelo == "") { request.vehiculo.modelo = "X"; }
                if (request.vehiculo.tipo == "") { request.vehiculo.tipo = "X"; }
                if (request.vehiculo.nomeclaturamotor == "") { request.vehiculo.nomeclaturamotor = "X"; }
                if (request.vehiculo.estado == "") { request.vehiculo.estado = "X"; }
                if (request.vehiculo.fechaRegistro == "") { request.vehiculo.fechaRegistro = "01/01/1999"; } // Formato fecha: DD/MM/YYYY HH24:MI:SS

                // Validar tamaño parametros: 
                mensajeAdvertencia = this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (request.vehiculo.fechaRegistro != "01/01/1999")
                {
                    if (!this.EsFecha(request.vehiculo.fechaRegistro)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
                }
            }

            #endregion

            if (mensajeAdvertencia == "")
            {
                try
                {
                    OracleCommand objCmd = new OracleCommand();
                    objCmd.Connection = (OracleConnection)connection;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_MCR_PROCES_VEHICULO";

                    objCmd.Parameters.Add("VEHID", OracleDbType.Int32).Value = request.vehiculo.id;
                    objCmd.Parameters.Add("VEHPLACA", OracleDbType.Varchar2).Value = request.vehiculo.placa;
                    objCmd.Parameters.Add("VEHMODELO", OracleDbType.Varchar2).Value = request.vehiculo.modelo;
                    objCmd.Parameters.Add("VEHKILOMETRAJE", OracleDbType.Int32).Value = request.vehiculo.kilometraje;
                    objCmd.Parameters.Add("VEHTIPO", OracleDbType.Varchar2).Value = request.vehiculo.tipo;
                    objCmd.Parameters.Add("VEHNOMECLATURA", OracleDbType.Varchar2).Value = request.vehiculo.nomeclaturamotor;
                    objCmd.Parameters.Add("VEHLOCLATITUD", OracleDbType.Varchar2).Value = request.vehiculo.locLatitudVehiculo;
                    objCmd.Parameters.Add("VEHLOCLONGITUD", OracleDbType.Varchar2).Value = request.vehiculo.locLongitudVehiculo;
                    objCmd.Parameters.Add("VEHOBSERVACIONES", OracleDbType.Varchar2).Value = request.vehiculo.observaciones;
                    objCmd.Parameters.Add("VEHESTADO", OracleDbType.Varchar2).Value = request.vehiculo.estado;
                    objCmd.Parameters.Add("VEHSUARIOCREACION", OracleDbType.Varchar2).Value = request.vehiculo.usuario;
                    objCmd.Parameters.Add("PROCESO", OracleDbType.Int32).Value = request.vehiculo.proceso;

                    // CRUD Roles-- > Buscar(1), Insertar(2), Actualizar(3), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
                    decimal opcionCRUD = request.vehiculo.proceso;

                    OracleDataReader reader = objCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        VehiculoDTO vehiculo = new VehiculoDTO();

                        vehiculo.id = (Int64)reader["VEHID"];
                        vehiculo.placa = (string)reader["VEHPLACA"].ToString();
                        vehiculo.modelo = (string)reader["VEHMODELO"].ToString();
                        vehiculo.kilometraje = (Int32)reader["VEHKILOMETRAJE"];
                        vehiculo.tipo = (string)reader["VEHTIPO"].ToString();  //user.activo = 'S';
                        vehiculo.nomeclaturamotor = (string)reader["VEHNOMECLATURA"].ToString();
                        vehiculo.locLatitudVehiculo = (string)reader["VEHLOCLATITUD"].ToString();
                        vehiculo.locLongitudVehiculo = (string)reader["VEHLOCLONGITUD"].ToString();
                        vehiculo.observaciones = (string)reader["VEHOBSERVACIONES"].ToString();
                        vehiculo.estado = (string)reader["VEHESTADO"].ToString();
                        vehiculo.fechaRegistro = DBNull.Value.Equals(reader["VEHFECHAREGISTRO"]) ? DateTime.Now.ToString() : ((DateTime)reader["VEHFECHAREGISTRO"]).ToString();                        
                        vehiculo.usuario = (string)reader["VEHSUARIOCREACION"];
                        vehiculo.mensaje = (string)reader["MENSAJE"].ToString();
                        vehiculo.proceso = (decimal)reader["PROCESO"];

                        listVehiculo.Add(vehiculo);
                    }

                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.ToString();

                    VehiculoDTO vehiculoErrorBD = new VehiculoDTO();

                    vehiculoErrorBD.id = 0;
                    vehiculoErrorBD.placa = "";
                    vehiculoErrorBD.modelo = "";
                    vehiculoErrorBD.kilometraje = 0;
                    vehiculoErrorBD.tipo = "";  //user.activo = 'S';
                    vehiculoErrorBD.nomeclaturamotor = "";
                    vehiculoErrorBD.locLatitudVehiculo = "";
                    vehiculoErrorBD.locLongitudVehiculo = "";                    
                    vehiculoErrorBD.observaciones = "";
                    vehiculoErrorBD.estado = "";
                    vehiculoErrorBD.fechaRegistro = "";
                    vehiculoErrorBD.proceso = request.vehiculo.proceso;
                    vehiculoErrorBD.usuario = "";                    
                    vehiculoErrorBD.mensaje = "Error en BD: " + mensaje.Substring(0, 200);

                    listVehiculoAdvertencia.Add(vehiculoErrorBD);

                    return listVehiculoAdvertencia;
                }

                return listVehiculo;
            }
            else
            {
                VehiculoDTO vehiculoAdvertenciaBD = new VehiculoDTO();

                vehiculoAdvertenciaBD.id = 0;
                vehiculoAdvertenciaBD.placa = "";
                vehiculoAdvertenciaBD.modelo = "";
                vehiculoAdvertenciaBD.kilometraje = 0;
                vehiculoAdvertenciaBD.tipo = "";  //user.activo = 'S';
                vehiculoAdvertenciaBD.nomeclaturamotor = "";
                vehiculoAdvertenciaBD.locLatitudVehiculo = "";
                vehiculoAdvertenciaBD.locLongitudVehiculo = "";
                vehiculoAdvertenciaBD.observaciones = "";
                vehiculoAdvertenciaBD.estado = "";
                vehiculoAdvertenciaBD.fechaRegistro = "";
                vehiculoAdvertenciaBD.proceso = request.vehiculo.proceso;
                vehiculoAdvertenciaBD.usuario = "";
                vehiculoAdvertenciaBD.mensaje = "Advertencia: " + mensajeAdvertencia;

                listVehiculoAdvertencia.Add(vehiculoAdvertenciaBD);

                return listVehiculoAdvertencia;
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
        public string validarTamanoParametros(VehiculoQuery request)
        {
            // Validar tamaño de parametros:
            if (request.vehiculo.placa.Length > 8) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro nombre no puede ser mayora a 8. "; }
            if (request.vehiculo.modelo.Length > 20) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro modelo no puede ser mayora a 20. "; }
            if (request.vehiculo.tipo.Length > 10) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro tipo no puede ser mayora a 10. "; }
            if (request.vehiculo.nomeclaturamotor.Length > 50) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro nomeclatura no puede ser mayora a 50. "; }
            if (request.vehiculo.estado.Length > 16) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro estado no puede ser mayora a 16. "; }
            if (request.vehiculo.fechaRegistro.Length > 10) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro fecha no puede ser mayora a 10, y debe cumplir con el formato DD/MM/YYYY. "; }

            return mensajeAdvertencia;
        }
    }
}
