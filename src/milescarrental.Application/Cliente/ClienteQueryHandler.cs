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

namespace milescarrental.Application.Cliente
{
    public class ClienteQueryHandler : IRequestHandler<ClienteQuery, List<ClienteDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        string mensajeAdvertencia = "";
        List<ClienteDTO> listClienteAdvertencia = new List<ClienteDTO>();

        public ClienteQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<ClienteDTO>> Handle(ClienteQuery request, CancellationToken cancellationToken)
        {
            List<ClienteDTO> listCliente = new List<ClienteDTO>();
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            #region Preparacion de Datos segun sea el Proceso:            

            // Procesos Vehiculo -- > Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar i Inactivar(4), Listar todos(5), Busqueda Generica(6)
            if (request.cliente.proceso < 1 || request.cliente.proceso > 6)
            {
                mensajeAdvertencia = "El parametro proceso debe estar entre 1 y 6. ";
            }

            // BUSQUEDA EXACTA
            if (request.cliente.proceso == 1)
            {
                // Validar parametros obligatorios:
                if (request.cliente.tipoDocumento == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar tipo de documento. "; }
                if (request.cliente.nroDocumento == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el numero de documento. "; }
                if (request.cliente.nombreCompleto == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre completo. "; }
                if (request.cliente.telefono == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar telefono. "; }
                if (request.cliente.direccion == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar direcion. "; }
                if (request.cliente.email == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar email. "; }
                if (request.cliente.observaciones == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar observaciones. "; }
                if (request.cliente.estado == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado del cliente (ACTIVO, INACTIVO, SUSPENDIDO). "; }
                if (request.cliente.usuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el usario que hace el registro. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.cliente.mensaje = "";
                    request.cliente.fechaRegistro = "01/01/1999";
                }
            }

            // INSERTAR
            if (request.cliente.proceso == 2)
            {
                // Validar parametros obligatorios:
                if (request.cliente.tipoDocumento == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar tipo de documento. "; }
                if (request.cliente.nroDocumento == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el numero de documento. "; }
                if (request.cliente.nombreCompleto == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre completo. "; }
                if (request.cliente.telefono == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar telefono. "; }
                if (request.cliente.direccion == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar direcion. "; }
                if (request.cliente.email == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar email. "; }
                if (request.cliente.observaciones == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar observaciones. "; }
                if (request.cliente.estado == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado del cliente (ACTIVO, INACTIVO, SUSPENDIDO). "; }
                if (request.cliente.usuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el usario que hace el registro. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.cliente.mensaje = "";
                    request.cliente.fechaRegistro = "01/01/1999";
                }
            }

            // ACTUALIZAR
            if (request.cliente.proceso == 3)
            {
                // Validar parametros obligatorios:
                if (request.cliente.tipoDocumento == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar tipo de documento. "; }
                if (request.cliente.nroDocumento == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el numero de documento. "; }
                if (request.cliente.nombreCompleto == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre completo. "; }
                if (request.cliente.telefono == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar telefono. "; }
                if (request.cliente.direccion == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar direcion. "; }
                if (request.cliente.email == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar email. "; }
                if (request.cliente.observaciones == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar observaciones. "; }
                if (request.cliente.estado == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado del cliente (ACTIVO, INACTIVO, SUSPENDIDO). "; }
                if (request.cliente.usuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el usario que hace el registro. "; }

                // Valores por defecto:
                if (request.cliente.fechaRegistro == "") { request.cliente.fechaRegistro = "01/01/1999"; }

                // Validar tamaño de parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (!this.EsFecha(request.cliente.fechaRegistro)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
                if (!(request.cliente.estado == "NUEVO" || request.cliente.estado == "USADO")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro tipo incorrecto (NUEVO, USADO) "; }
            }

            // ELIMINAR O INACTIVAR
            if (request.cliente.proceso == 4)
            {
                // Validar parametros obligatorios:
                if (request.cliente.id < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el id. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.cliente.tipoDocumento = "X";
                    request.cliente.nroDocumento = "X";
                    request.cliente.nombreCompleto = "X";
                    request.cliente.telefono = "X";
                    request.cliente.direccion = "X";
                    request.cliente.email = "X";
                    request.cliente.observaciones = "X";
                    request.cliente.estado = "X";
                    request.cliente.fechaRegistro = "01/01/1999";
                }
            }

            // LISTAR TODOS
            if (request.cliente.proceso == 5)
            {
                // Valores por defecto:
                request.cliente.id = 0;
                request.cliente.tipoDocumento = "X";
                request.cliente.nroDocumento = "X";
                request.cliente.nombreCompleto = "X";
                request.cliente.telefono = "X";
                request.cliente.direccion = "X";
                request.cliente.email = "X";
                request.cliente.observaciones = "X";
                request.cliente.estado = "X";
                request.cliente.fechaRegistro = "01/01/1999";
            }

            // BUSQUEDA GENERICA
            if (request.cliente.proceso == 6)
            {
                // Valores por defecto:
                if (request.cliente.tipoDocumento == "") { request.cliente.tipoDocumento = "X"; }
                if (request.cliente.nroDocumento == "") { request.cliente.nroDocumento = "X"; }
                if (request.cliente.nombreCompleto == "") { request.cliente.nombreCompleto = "X"; }
                if (request.cliente.telefono == "") { request.cliente.telefono = "X"; }
                if (request.cliente.direccion == "") { request.cliente.direccion = "X"; }
                if (request.cliente.email == "") { request.cliente.email = "X"; }
                if (request.cliente.observaciones == "") { request.cliente.observaciones = "X"; }
                if (request.cliente.estado == "") { request.cliente.estado = "X"; }
                if (request.cliente.fechaRegistro == "") { request.cliente.fechaRegistro = "01/01/1999"; } // Formato fecha: DD/MM/YYYY HH24:MI:SS

                // Validar tamaño parametros: 
                mensajeAdvertencia = this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (request.cliente.fechaRegistro != "01/01/1999")
                {
                    if (!this.EsFecha(request.cliente.fechaRegistro)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
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
                    objCmd.CommandText = "PR_MCR_PROCES_CLIENTE";

                    objCmd.Parameters.Add("CLIID", OracleDbType.Int32).Value = request.cliente.id;
                    objCmd.Parameters.Add("CLITIPODOCUMENTO", OracleDbType.Varchar2).Value = request.cliente.tipoDocumento;
                    objCmd.Parameters.Add("CLINRODOCUMENTO", OracleDbType.Varchar2).Value = request.cliente.nroDocumento;
                    objCmd.Parameters.Add("CLINOMBRECOMPLETO", OracleDbType.Varchar2).Value = request.cliente.nombreCompleto;
                    objCmd.Parameters.Add("CLITELEFONO", OracleDbType.Varchar2).Value = request.cliente.telefono;
                    objCmd.Parameters.Add("CLIDIRECCION", OracleDbType.Varchar2).Value = request.cliente.direccion;
                    objCmd.Parameters.Add("CLIEMAIL", OracleDbType.Varchar2).Value = request.cliente.email;
                    objCmd.Parameters.Add("CLIOBSERVACIONES", OracleDbType.Varchar2).Value = request.cliente.observaciones;
                    objCmd.Parameters.Add("CLIESTADO", OracleDbType.Varchar2).Value = request.cliente.estado;
                    objCmd.Parameters.Add("CLISUARIOCREACION", OracleDbType.Varchar2).Value = request.cliente.usuario;
                    objCmd.Parameters.Add("PROCESO", OracleDbType.Int32).Value = request.cliente.proceso;

                    // CRUD Roles-- > Buscar(1), Insertar(2), Actualizar(3), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
                    decimal opcionCRUD = request.cliente.proceso;

                    OracleDataReader reader = objCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ClienteDTO cliente = new ClienteDTO();

                        cliente.id = (Int64)reader["CLIID"];
                        cliente.tipoDocumento = (string)reader["CLITIPODOCUMENTO"].ToString();
                        cliente.nroDocumento = (string)reader["CLINRODOCUMENTO"].ToString();
                        cliente.nombreCompleto = (string)reader["CLINOMBRECOMPLETO"];
                        cliente.telefono = (string)reader["CLITELEFONO"].ToString();  //user.activo = 'S';
                        cliente.direccion = (string)reader["CLIDIRECCION"].ToString();
                        cliente.email = (string)reader["CLIEMAIL"].ToString();
                        cliente.observaciones = (string)reader["CLIOBSERVACIONES"].ToString();
                        cliente.estado = (string)reader["CLIESTADO"].ToString();
                        cliente.fechaRegistro = DBNull.Value.Equals(reader["CLIFECHAREGISTRO"]) ? DateTime.Now.ToString() : ((DateTime)reader["CLIFECHAREGISTRO"]).ToString();
                        cliente.proceso = (decimal)reader["PROCESO"];
                        cliente.usuario = (string)reader["CLISUARIOCREACION"];
                        cliente.mensaje = (string)reader["MENSAJE"].ToString();
                        
                        listCliente.Add(cliente);
                    }

                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.ToString();

                    ClienteDTO clienteErrorBD = new ClienteDTO();

                    clienteErrorBD.id = 0;
                    clienteErrorBD.tipoDocumento = "";
                    clienteErrorBD.nroDocumento = "";
                    clienteErrorBD.nombreCompleto = "";
                    clienteErrorBD.telefono = "";
                    clienteErrorBD.direccion = "";
                    clienteErrorBD.email = "";
                    clienteErrorBD.observaciones = "";
                    clienteErrorBD.estado = "";
                    clienteErrorBD.fechaRegistro = "";
                    clienteErrorBD.proceso = request.cliente.proceso;
                    clienteErrorBD.usuario = "";
                    clienteErrorBD.mensaje = "Error en BD: " + mensaje.Substring(0, 200);

                    listClienteAdvertencia.Add(clienteErrorBD);

                    return listClienteAdvertencia;
                }

                return listCliente;
            }
            else
            {
                ClienteDTO clienteAdvertenciaBD = new ClienteDTO();

                clienteAdvertenciaBD.id = 0;
                clienteAdvertenciaBD.tipoDocumento = "";
                clienteAdvertenciaBD.nroDocumento = "";
                clienteAdvertenciaBD.nombreCompleto = "";
                clienteAdvertenciaBD.telefono = "";
                clienteAdvertenciaBD.direccion = "";
                clienteAdvertenciaBD.email = "";
                clienteAdvertenciaBD.observaciones = "";
                clienteAdvertenciaBD.estado = "";
                clienteAdvertenciaBD.fechaRegistro = "";
                clienteAdvertenciaBD.proceso = request.cliente.proceso;
                clienteAdvertenciaBD.usuario = "";
                clienteAdvertenciaBD.mensaje = "Advertencia: " + mensajeAdvertencia;

                listClienteAdvertencia.Add(clienteAdvertenciaBD);

                return listClienteAdvertencia;
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
        public string validarTamanoParametros(ClienteQuery request)
        {
            // Validar tamaño de parametros:
            if (request.cliente.nroDocumento.Length > 20) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro nombre no puede ser mayora a 8. "; }
            if (request.cliente.tipoDocumento.Length > 20) { mensajeAdvertencia = mensajeAdvertencia + "El tamaño del parametro modelo no puede ser mayora a 20. "; }

            return mensajeAdvertencia;
        }
    }
}
