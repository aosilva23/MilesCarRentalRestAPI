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
    public class UsuarioRolQueryHandler : IRequestHandler<UsuarioRolQuery, List<UsuarioRolDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        List<UsuarioRolDTO> listUsuariosRolesAdvertencia = new List<UsuarioRolDTO>();
        string mensajeAdvertencia = "";

        public UsuarioRolQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<UsuarioRolDTO>> Handle(UsuarioRolQuery request, CancellationToken cancellationToken)
        {
            List<UsuarioRolDTO> listUsuariosRoles = new List<UsuarioRolDTO>();
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            #region Preparacion de Datos segun sea el caso CRUD:            

            // CRUD Usuarios-- > Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar i Inactivar(4), Listar todos(5), Busqueda Generica(6)
            if (request.usuariorol.idcrud < 1 || request.usuariorol.idcrud > 6 || request.usuariorol.idcrud == 3)
            {
                mensajeAdvertencia = "El parametro idcrud debe ser 1, 2, 4, 5 o 6. ";
            }

            // BUSQUEDA EXACTA
            if (request.usuariorol.idcrud == 1)
            {
                // Validar parametros obligatorios:
                if (request.usuariorol.nombreUsuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre de usuario. "; }
                if (request.usuariorol.rolId < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el id del rol. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);
            }

            // INSERTAR
            if (request.usuariorol.idcrud == 2)
            {
                // Validar parametros obligatorios:
                if (request.usuariorol.nombreUsuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre de usuario. "; }
                if (request.usuariorol.rolId < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el id del rol. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);
            }


            // ELIMINAR
            if (request.usuariorol.idcrud == 4)
            {
                // Validar parametros obligatorios:
                if (request.usuariorol.nombreUsuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre de usuario. "; }
                if (request.usuariorol.rolId < 1) { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el id del rol. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);
            }

            // LISTAR TODOS
            if (request.usuariorol.idcrud == 5)
            {
                // Valores por defecto:
                request.usuariorol.nombreUsuario = "XXXXXX";
                request.usuariorol.nombreRol = "XXXXXX";
                request.usuariorol.rolId = 9999;
            }

            // BUSQUEDA GENERICA
            if (request.usuariorol.idcrud == 6)
            {
                if (request.usuariorol.nombreUsuario == "") { request.usuariorol.nombreUsuario = "XXXXXX"; }
                if (request.usuariorol.nombreRol == "") { request.usuariorol.nombreRol = "XXXXXX"; }
                if (request.usuariorol.rolId < 1) { request.usuariorol.rolId = 0; }                

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);
            }

            #endregion


            if (mensajeAdvertencia == "")
            {
                try
                {
                    OracleCommand objCmd = new OracleCommand();
                    objCmd.Connection = (OracleConnection)connection;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "PR_MIG_CRUD_USUARIOROL";

                    objCmd.Parameters.Add("IDROL", OracleDbType.Int32).Value = request.usuariorol.rolId;
                    objCmd.Parameters.Add("NOMBREUSUARIO", OracleDbType.Varchar2).Value = request.usuariorol.nombreUsuario;
                    objCmd.Parameters.Add("IDCRUD", OracleDbType.Int32).Value = request.usuariorol.idcrud;

                    // CRUD Usuarios-- > Buscar(1), Insertar(2), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
                    decimal opcionCRUD = request.usuariorol.idcrud;

                    OracleDataReader reader = objCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UsuarioRolDTO usuariorol = new UsuarioRolDTO();

                        usuariorol.rolId = Convert.ToInt32(reader["RUSIDROL"]);
                        usuariorol.nombreRol = (reader["RUSNOMBREROL"] != DBNull.Value) ? (string)reader["RUSNOMBREROL"] : "";
                        usuariorol.nombreUsuario = (reader["RUSNOMBREUSUARIO"] != DBNull.Value) ? (string)reader["RUSNOMBREUSUARIO"] : "";
                        usuariorol.idcrud = Convert.ToInt32(reader["IDCRUD"]);
                        usuariorol.mensaje = (string)reader["MENSAJE"];

                        listUsuariosRoles.Add(usuariorol);
                    }

                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.ToString();

                    UsuarioRolDTO usuarioRolErrorBD = new UsuarioRolDTO();

                    usuarioRolErrorBD.rolId = 0;
                    usuarioRolErrorBD.nombreRol = "";
                    
                    usuarioRolErrorBD.idcrud = request.usuariorol.idcrud;
                    usuarioRolErrorBD.mensaje = "Error en BD: " + mensaje.Substring(0, 200);

                    listUsuariosRolesAdvertencia.Add(usuarioRolErrorBD);

                    return listUsuariosRolesAdvertencia;
                }

                return listUsuariosRoles;
            }
            else
            {
                UsuarioRolDTO usuarioRolErrorBD = new UsuarioRolDTO();

                usuarioRolErrorBD.rolId = 0;
                usuarioRolErrorBD.nombreRol = "";

                usuarioRolErrorBD.idcrud = request.usuariorol.idcrud;
                usuarioRolErrorBD.mensaje = "Advertencia: " + mensajeAdvertencia;

                listUsuariosRolesAdvertencia.Add(usuarioRolErrorBD);

                return listUsuariosRolesAdvertencia;
            }            
        }

        public string validarTamanoParametros(UsuarioRolQuery request)
        {
            if (request.usuariorol.nombreUsuario.Length > 100) { mensajeAdvertencia = "El tamaño del parametro nombre usuario no puede ser mayora a 100. "; }

            return mensajeAdvertencia;
        }
    }
}
