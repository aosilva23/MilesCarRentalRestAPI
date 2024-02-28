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

namespace milescarrental.Application.PermisosAcceso
{
    public class OpcionMenuQueryHandler : IRequestHandler<OpcionMenuQuery, List<OpcionMenuDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        string mensajeAdvertencia = "";
        List<OpcionMenuDTO> listOpcionMenuAdvertencia = new List<OpcionMenuDTO>();

        public OpcionMenuQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<OpcionMenuDTO>> Handle(OpcionMenuQuery request, CancellationToken cancellationToken)
        {
            List<OpcionMenuDTO> listOpcionesMenu = new List<OpcionMenuDTO>();
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            #region Preparacion de Datos segun sea el caso CRUD:            

            // CRUD Usuarios-- > Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar i Inactivar(4), Listar todos(5), Busqueda Generica(6)
            if (request.opcionMenu.idcrud < 1 || request.opcionMenu.idcrud > 6)
            {
                mensajeAdvertencia = "El parametro idcrud debe estar entre 1 y 6. ";
            }

            // BUSQUEDA EXACTA
            if (request.opcionMenu.idcrud == 1)
            {
                // Validar parametros obligatorios:
                if (request.opcionMenu.nombre == "") { mensajeAdvertencia = "Ingresar el nombre. "; }
                if (request.opcionMenu.url == "") { mensajeAdvertencia = "Ingresar la Url. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.opcionMenu.activo = "X";
                    request.opcionMenu.usuarioCreacion = "XXXXXX";
                    request.opcionMenu.fechaCreacion = "01/01/1999";
                }
            }

            // INSERTAR
            if (request.opcionMenu.idcrud == 2)
            {
                // Validar parametros obligatorios:
                if (request.opcionMenu.nombre == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre. "; }
                if (request.opcionMenu.url == "") { mensajeAdvertencia = "Ingresar la Url. "; }
                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.opcionMenu.activo = "S";
                    request.opcionMenu.fechaCreacion = "01/01/1999";
                }
            }

            // ACTUALIZAR
            if (request.opcionMenu.idcrud == 3)
            {
                // Validar parametros obligatorios:
                if (request.opcionMenu.id < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el id del rol. "; }
                if (request.opcionMenu.nombre == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre. "; }
                if (request.opcionMenu.url == "") { mensajeAdvertencia = "Ingresar la Url. "; }
                if (request.opcionMenu.activo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado activo (S/N). "; }

                // Valores por defecto:
                if (request.opcionMenu.fechaCreacion == "") { request.opcionMenu.fechaCreacion = "01/01/1999"; }

                // Validar tamaño de parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (!this.EsFecha(request.opcionMenu.fechaCreacion)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
                if (!(request.opcionMenu.activo == "S" || request.opcionMenu.activo == "N")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro activo incorrecto (S/N) "; }
            }

            // ELIMINAR O INACTIVAR
            if (request.opcionMenu.idcrud == 4)
            {
                // Validar parametros obligatorios:
                if (request.opcionMenu.id < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el id. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.opcionMenu.nombre = "XXXXXX";
                    request.opcionMenu.url = "XXXXXX";
                    request.opcionMenu.activo = "X";
                    request.opcionMenu.usuarioCreacion = "XXXXXX";
                    request.opcionMenu.fechaCreacion = "01/01/1999";
                }
            }

            // LISTAR TODOS
            if (request.opcionMenu.idcrud == 5)
            {
                // Valores por defecto:
                request.opcionMenu.id = 0;
                request.opcionMenu.nombre = "XXXXXX";
                request.opcionMenu.url = "XXXXXX";
                request.opcionMenu.activo = "X";
                request.opcionMenu.usuarioCreacion = "XXXXXX";
                request.opcionMenu.fechaCreacion = "01/01/1999";
            }

            // BUSQUEDA GENERICA
            if (request.opcionMenu.idcrud == 6)
            {
                // Valores por defecto:
                if (request.opcionMenu.nombre == "") { request.opcionMenu.nombre = "XXXXXX"; }
                if (request.opcionMenu.url == "") { request.opcionMenu.url = "XXXXXX"; }
                if (request.opcionMenu.activo == "") { request.opcionMenu.activo = "X"; }
                if (request.opcionMenu.usuarioCreacion == "") { request.opcionMenu.usuarioCreacion = "XXXXXX"; }
                if (request.opcionMenu.fechaCreacion == "") { request.opcionMenu.fechaCreacion = "01/01/1999"; } // Formato fecha: DD/MM/YYYY HH24:MI:SS

                // Validar tamaño parametros: 
                mensajeAdvertencia = this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (request.opcionMenu.fechaCreacion != "01/01/1999")
                {
                    if (!this.EsFecha(request.opcionMenu.fechaCreacion)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
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
                    objCmd.CommandText = "PR_MIG_CRUD_OPCIONMENU";

                    objCmd.Parameters.Add("ID", OracleDbType.Int32).Value = request.opcionMenu.id;
                    objCmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2).Value = request.opcionMenu.nombre;
                    objCmd.Parameters.Add("URL", OracleDbType.Varchar2).Value = request.opcionMenu.url;
                    objCmd.Parameters.Add("ACTIVO", OracleDbType.Varchar2).Value = request.opcionMenu.activo;
                    objCmd.Parameters.Add("USUARIOCREACION", OracleDbType.Varchar2).Value = request.opcionMenu.usuarioCreacion;
                    objCmd.Parameters.Add("IDCRUD", OracleDbType.Int32).Value = request.opcionMenu.idcrud;

                    // CRUD Roles-- > Buscar(1), Insertar(2), Actualizar(3), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
                    decimal opcionCRUD = request.opcionMenu.idcrud;

                    OracleDataReader reader = objCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        OpcionMenuDTO opcionMenu = new OpcionMenuDTO();
                        opcionMenu.id = (Int16)reader["OPMID"];
                        opcionMenu.nombre = (string)reader["OPMNOMBREOPCION"].ToString();
                        opcionMenu.url = (string)reader["OPMURLOPCION"].ToString();
                        opcionMenu.activo = (string)reader["OPMACTIVO"].ToString();  //user.activo = 'S';
                        opcionMenu.usuarioCreacion = (string)reader["OPMUSUARIOCREACION"].ToString();
                        opcionMenu.fechaCreacion = DBNull.Value.Equals(reader["OPMFECHACREACION"]) ? DateTime.Now.ToString() : ((DateTime)reader["OPMFECHACREACION"]).ToString();
                        opcionMenu.idcrud = (decimal)reader["IDCRUD"];
                        opcionMenu.mensaje = (string)reader["MENSAJE"].ToString();
                        listOpcionesMenu.Add(opcionMenu);
                    }

                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.ToString();

                    OpcionMenuDTO opcionMenuErrorBD = new OpcionMenuDTO();

                    opcionMenuErrorBD.id = 0;
                    opcionMenuErrorBD.nombre = "";
                    opcionMenuErrorBD.url = "";
                    opcionMenuErrorBD.activo = "";
                    opcionMenuErrorBD.usuarioCreacion = "";

                    opcionMenuErrorBD.idcrud = request.opcionMenu.idcrud;
                    opcionMenuErrorBD.mensaje = "Error en BD: " + mensaje.Substring(0, 200);

                    listOpcionMenuAdvertencia.Add(opcionMenuErrorBD);

                    return listOpcionMenuAdvertencia;
                }

                return listOpcionesMenu;
            }
            else
            {
                OpcionMenuDTO opcionMenu = new OpcionMenuDTO();

                opcionMenu.id = 0;
                opcionMenu.nombre = "";
                opcionMenu.url = "";
                opcionMenu.activo = "";
                opcionMenu.usuarioCreacion = "";

                opcionMenu.idcrud = request.opcionMenu.idcrud;
                opcionMenu.mensaje = "Advertencia: " + mensajeAdvertencia;

                listOpcionMenuAdvertencia.Add(opcionMenu);

                return listOpcionMenuAdvertencia;
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
        public string validarTamanoParametros(OpcionMenuQuery request)
        {
            // Validar tamaño de parametros:
            if (request.opcionMenu.nombre.Length > 100) { mensajeAdvertencia = "El tamaño del parametro nombre no puede ser mayora a 100. "; }
            if (request.opcionMenu.url.Length > 200) { mensajeAdvertencia = "El tamaño del parametro nombre Url no puede ser mayora a 200. "; }
            if (request.opcionMenu.activo.Length > 1) { mensajeAdvertencia = "El tamaño del parametro activo no puede ser mayora a 1. "; }
            if (request.opcionMenu.usuarioCreacion.Length > 50) { mensajeAdvertencia = "El tamaño del parametro usuario creacion no puede ser mayora a 50. "; }
            if (request.opcionMenu.fechaCreacion.Length > 10) { mensajeAdvertencia = "El tamaño del parametro fecha creacion no puede ser mayora a 10 (DD/MM/YYYY). "; }

            return mensajeAdvertencia;
        }
    }
}
