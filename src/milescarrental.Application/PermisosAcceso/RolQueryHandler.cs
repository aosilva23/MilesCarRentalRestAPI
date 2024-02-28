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
    public class RolQueryHandler : IRequestHandler<RolQuery, List<RolDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        string mensajeAdvertencia = "";
        List<RolDTO> listRolesAdvertencia = new List<RolDTO>();

        public RolQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<RolDTO>> Handle(RolQuery request, CancellationToken cancellationToken)
        {
            List<RolDTO> listRoles = new List<RolDTO>();
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            #region Preparacion de Datos segun sea el caso CRUD:            

            // CRUD Usuarios-- > Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar i Inactivar(4), Listar todos(5), Busqueda Generica(6)
            if (request.rol.idcrud < 1 || request.rol.idcrud > 6)
            {
                mensajeAdvertencia = "El parametro idcrud debe estar entre 1 y 6. ";
            }

            // BUSQUEDA EXACTA
            if (request.rol.idcrud == 1)
            {
                // Validar parametros obligatorios:
                if (request.rol.nombre == "") { mensajeAdvertencia = "Ingresar el nombre. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.rol.activo = "X";
                    request.rol.usuarioCreacion = "XXXXXX";
                    request.rol.fechaCreacion = "01/01/1999";
                }
            }

            // INSERTAR
            if (request.rol.idcrud == 2)
            {
                // Validar parametros obligatorios:
                if (request.rol.nombre == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.rol.activo = "S";
                    request.rol.fechaCreacion = "01/01/1999";
                }
            }

            // ACTUALIZAR
            if (request.rol.idcrud == 3)
            {
                // Validar parametros obligatorios:
                if (request.rol.id < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el id del rol. "; }
                if (request.rol.nombre == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre. "; }
                if (request.rol.activo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado activo (S/N). "; }

                // Valores por defecto:
                if (request.rol.fechaCreacion == "") { request.rol.fechaCreacion = "01/01/1999"; }

                // Validar tamaño de parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (!this.EsFecha(request.rol.fechaCreacion)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
                if (!(request.rol.activo == "S" || request.rol.activo == "N")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro activo incorrecto (S/N) "; }
            }

            // ELIMINAR O INACTIVAR
            if (request.rol.idcrud == 4)
            {
                // Validar parametros obligatorios:
                if (request.rol.nombre == "") { mensajeAdvertencia = mensajeAdvertencia +  "Ingresar el nombre de rol."; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.rol.activo = "X";
                    request.rol.usuarioCreacion = "XXXXXX";
                    request.rol.fechaCreacion = "01/01/1999";
                }
            }

            // LISTAR TODOS
            if (request.rol.idcrud == 5)
            {
                // Valores por defecto:
                request.rol.nombre = "XXXXXX";
                request.rol.activo = "X";
                request.rol.usuarioCreacion = "XXXXXX";
                request.rol.fechaCreacion = "01/01/1999";
            }

            // BUSQUEDA GENERICA
            if (request.rol.idcrud == 6)
            {
                // Valores por defecto:
                if (request.rol.nombre == "") { request.rol.nombre = "XXXXXX"; }
                if (request.rol.activo == "") { request.rol.activo = "X"; }
                if (request.rol.usuarioCreacion == "") { request.rol.usuarioCreacion = "XXXXXX"; }
                if (request.rol.fechaCreacion == "") { request.rol.fechaCreacion = "01/01/1999"; } // Formato fecha: DD/MM/YYYY HH24:MI:SS

                // Validar tamaño parametros: 
                mensajeAdvertencia = this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (request.rol.fechaCreacion != "01/01/1999")
                {
                    if (!this.EsFecha(request.rol.fechaCreacion)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
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
                    objCmd.CommandText = "PR_MIG_CRUD_ROL";

                    objCmd.Parameters.Add("ID", OracleDbType.Int32).Value = request.rol.id;
                    objCmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2).Value = request.rol.nombre;
                    objCmd.Parameters.Add("ACTIVO", OracleDbType.Varchar2).Value = request.rol.activo;
                    objCmd.Parameters.Add("USUARIOCREACION", OracleDbType.Varchar2).Value = request.rol.usuarioCreacion;
                    objCmd.Parameters.Add("IDCRUD", OracleDbType.Int32).Value = request.rol.idcrud;

                    // CRUD Roles-- > Buscar(1), Insertar(2), Actualizar(3), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
                    decimal opcionCRUD = request.rol.idcrud;

                    OracleDataReader reader = objCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        RolDTO rol = new RolDTO();
                        rol.id = (Int16)reader["ROLID"];
                        rol.nombre = (string)reader["ROLNOMBRE"].ToString();
                        rol.activo = (string)reader["ROLACTIVO"].ToString();  //user.activo = 'S';
                        rol.usuarioCreacion = (string)reader["ROLUSUARIOCREACION"].ToString();
                        rol.fechaCreacion = DBNull.Value.Equals(reader["ROLFECHACREACION"]) ? DateTime.Now.ToString() : ((DateTime)reader["ROLFECHACREACION"]).ToString();
                        rol.idcrud = (decimal)reader["IDCRUD"];
                        rol.mensaje = (string)reader["MENSAJE"].ToString();
                        listRoles.Add(rol);
                    }                    

                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.ToString();

                    RolDTO rolErrorBD = new RolDTO();

                    // Parametros generalizados
                    rolErrorBD.id = 0;
                    rolErrorBD.nombre = "";
                    rolErrorBD.activo = "";
                    rolErrorBD.usuarioCreacion = "";
                    rolErrorBD.fechaCreacion = "";
                    rolErrorBD.idcrud = request.rol.idcrud;

                    // Parametros mensaje advertencia
                    rolErrorBD.mensaje = "Error en BD: " + mensaje.Substring(0, 200);

                    listRolesAdvertencia.Add(rolErrorBD);

                    return listRolesAdvertencia;
                }

                return listRoles;
            }
            else
            {
                RolDTO rol = new RolDTO();

                // Parametros generalizados
                rol.nombre = "";
                rol.activo = "";
                rol.usuarioCreacion = "";
                rol.fechaCreacion = "";

                // Parametros mensaje advertencia
                rol.idcrud = request.rol.idcrud;
                rol.mensaje = "Advertencia: " + mensajeAdvertencia;

                listRolesAdvertencia.Add(rol);

                return listRolesAdvertencia;
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
        public string validarTamanoParametros(RolQuery request)
        {
            // Validar tamaño de parametros:
            if (request.rol.nombre.Length > 100) { mensajeAdvertencia = "El tamaño del parametro nombre usuario no puede ser mayora a 100. "; }
            if (request.rol.activo.Length > 1) { mensajeAdvertencia = "El tamaño del parametro activo no puede ser mayora a 1. "; }
            if (request.rol.usuarioCreacion.Length > 50) { mensajeAdvertencia = "El tamaño del parametro usuario creacion no puede ser mayora a 50. "; }
            if (request.rol.fechaCreacion.Length > 10) { mensajeAdvertencia = "El tamaño del parametro fecha creacion no puede ser mayora a 10 (DD/MM/YYYY). "; }

            return mensajeAdvertencia;
        }
    }
}
