using Dapper;
using MediatR;
using Oracle.ManagedDataAccess.Client;
using milescarrental.Application.Configuration.Data;
using milescarrental.Application.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using milescarrental.Application.Helpers;

namespace milescarrental.Application.PermisosAcceso
{
    public class UsuarioQueryHandler : IRequestHandler<UsuarioQuery, List<UsuarioDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        List<UsuarioDTO> listUsuariosAdvertencia = new List<UsuarioDTO>();
        string mensajeAdvertencia = "";

        public UsuarioQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<UsuarioDTO>> Handle(UsuarioQuery request, CancellationToken cancellationToken)
        {
            string vector = _appConfig.Vector;
            //Encryptor encrypt = new Encryptor();
            EncryptorAes encrypt = new EncryptorAes();

            List<UsuarioDTO> listUsuarios = new List<UsuarioDTO>();
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            #region Preparacion de Datos segun sea el caso CRUD:

            // CRUD Usuarios-- > Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar i Inactivar(4), Listar todos(5), Busqueda Generica(6)
            if (request.Usuario.idcrud < 1 || request.Usuario.idcrud > 6) 
            { 
                mensajeAdvertencia = "El parametro idcrud debe estar entre 1 y 6. ";
            }

            // BUSQUEDA EXACTA
            if (request.Usuario.idcrud == 1)
            {
                // Validar parametros obligatorios:
                if (request.Usuario.clave == ""){mensajeAdvertencia = "Ingresar la clave. ";}
                if (request.Usuario.nombreUsuario == ""){mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre de usuario.";}

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    request.Usuario.clave = encrypt.EncriptarCadenaDeCaracteres(request.Usuario.clave, vector);

                    // Valores por defecto:
                    request.Usuario.nombre = "XXXXXX";
                    request.Usuario.apellido = "XXXXXX";
                    request.Usuario.correoElectronico = "XXXXXX";
                    request.Usuario.activo = "X";
                    request.Usuario.notificacionProceso = "X";
                    request.Usuario.proceso = "XXXXXX";
                    request.Usuario.usuarioCreacion = "XXXXXX";
                    request.Usuario.fechaCreacion = "01/01/1999";
                }
            }

            // INSERTAR
            if (request.Usuario.idcrud == 2)
            {
                // Validar parametros obligatorios:
                if (request.Usuario.nombreUsuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre de usuario. "; }
                if (request.Usuario.nombre == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre. "; }
                if (request.Usuario.apellido == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el apellido. "; }
                if (request.Usuario.correoElectronico == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el correo electronico. "; }
                if (request.Usuario.clave == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar la clave. "; }

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Restrincion hecha en la base de datos para el campo USUPROCESO:
                // CHECK (USUPROCESO in ('VALIDACION_ESTRUCTURA','VALIDACION_MIGRACION','AMBOS'))
                if (!(request.Usuario.proceso == "AMBOS" || request.Usuario.proceso == "")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro proceso incorrecto (AMBOS o vacio) "; }

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    request.Usuario.clave = encrypt.EncriptarCadenaDeCaracteres(request.Usuario.clave, vector);

                    // Valores por defecto:
                    request.Usuario.activo = "S";
                    request.Usuario.notificacionProceso = "S";
                    request.Usuario.fechaCreacion = "01/01/1999";
                }
            }

            // ACTUALIZAR
            if (request.Usuario.idcrud == 3)
            {
                // Validar parametros obligatorios:
                if (request.Usuario.clave == "") { mensajeAdvertencia = "Ingresar la clave. "; }
                if (request.Usuario.nombreUsuario == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre de usuario."; }
                if (request.Usuario.nombre == ""){ mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre. ";}
                if (request.Usuario.apellido == ""){ mensajeAdvertencia = mensajeAdvertencia + "Ingresar el apellido. ";}
                if (request.Usuario.correoElectronico == ""){ mensajeAdvertencia = mensajeAdvertencia + "Ingresar el correo electronico. ";}
                if (request.Usuario.activo == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado activo (S/N). "; }
                if (request.Usuario.notificacionProceso == "") { mensajeAdvertencia = mensajeAdvertencia + "Ingresar el estado notificacion proceso (S/N). "; }

                // Valores por defecto:
                if (request.Usuario.fechaCreacion == "") { request.Usuario.fechaCreacion = "01/01/1999"; }

                // Validar tamaño de parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (!this.EsFecha(request.Usuario.fechaCreacion)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
                if(!(request.Usuario.activo == "S" || request.Usuario.activo == "N")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro activo incorrecto (S/N) "; }
                if (!(request.Usuario.notificacionProceso == "S" || request.Usuario.notificacionProceso == "N")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro notificacion proceso incorrecto (S/N) "; }

                // Restrincion hecha en la base de datos para el campo USUPROCESO:
                // CHECK (USUPROCESO in ('VALIDACION_ESTRUCTURA','VALIDACION_MIGRACION','AMBOS'))
                if (!(request.Usuario.proceso == "AMBOS" || request.Usuario.proceso == "")) { mensajeAdvertencia = mensajeAdvertencia + "Formato del parametro proceso incorrecto (AMBOS o vacio) "; }                

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    request.Usuario.clave = encrypt.EncriptarCadenaDeCaracteres(request.Usuario.clave, vector);
                }
            }

            // ELIMINAR O INACTIVAR
            if (request.Usuario.idcrud == 4)
            {
                // Validar parametros obligatorios:
                if (request.Usuario.nombreUsuario == ""){mensajeAdvertencia = mensajeAdvertencia + "Ingresar el nombre de usuario.";}

                // Validar tamaño parametros:
                mensajeAdvertencia = mensajeAdvertencia + this.validarTamanoParametros(request);

                // Validar si hay mensajes de advertencia:
                if (mensajeAdvertencia == "")
                {
                    // Valores por defecto:
                    request.Usuario.clave = "XXXXXX";
                    request.Usuario.nombre = "XXXXXX";
                    request.Usuario.apellido = "XXXXXX";
                    request.Usuario.correoElectronico = "XXXXXX";
                    request.Usuario.activo = "X";
                    request.Usuario.notificacionProceso = "X";
                    request.Usuario.proceso = "XXXXXX";
                    request.Usuario.usuarioCreacion = "XXXXXX";
                    request.Usuario.fechaCreacion = "01/01/1999";
                }
            }

            // LISTAR TODOS
            if (request.Usuario.idcrud == 5)
            {
                // Valores por defecto:
                request.Usuario.clave = "XXXXXX";
                request.Usuario.nombreUsuario = "XXXXXX";
                request.Usuario.nombre = "XXXXXX";
                request.Usuario.apellido = "XXXXXX";
                request.Usuario.correoElectronico = "XXXXXX";
                request.Usuario.activo = "X";
                request.Usuario.notificacionProceso = "X";
                request.Usuario.proceso = "XXXXXX";
                request.Usuario.usuarioCreacion = "XXXXXX";
                request.Usuario.fechaCreacion = "01/01/1999";
            }

            // BUSQUEDA GENERICA
            if (request.Usuario.idcrud == 6)
            {
                // Valores por defecto:
                if (request.Usuario.nombreUsuario == "") { request.Usuario.nombreUsuario = "XXXXXX"; }
                if (request.Usuario.nombre == "") { request.Usuario.nombre = "XXXXXX"; }
                if (request.Usuario.apellido == "") { request.Usuario.apellido = "XXXXXX"; }
                if (request.Usuario.correoElectronico == "") { request.Usuario.correoElectronico = "XXXXXX"; }
                if (request.Usuario.clave == "") { request.Usuario.clave = "XXXXXX";}
                if (request.Usuario.activo == "") { request.Usuario.activo = "X"; }
                if (request.Usuario.notificacionProceso == "") { request.Usuario.notificacionProceso = "X"; }
                if (request.Usuario.proceso == "") { request.Usuario.proceso = "X"; }
                if (request.Usuario.usuarioCreacion == "") { request.Usuario.usuarioCreacion = "XXXXXX"; }
                if (request.Usuario.fechaCreacion == "") { request.Usuario.fechaCreacion = "01/01/1999"; } // Formato fecha: DD/MM/YYYY HH24:MI:SS

                // Validar tamaño parametros: 
                mensajeAdvertencia = this.validarTamanoParametros(request);

                // Validar formatos de parametros:
                if (request.Usuario.fechaCreacion != "01/01/1999")
                {
                    if (!this.EsFecha(request.Usuario.fechaCreacion)) { mensajeAdvertencia = mensajeAdvertencia + "Formato de fecha incorrecto, el formato es DD/MM/YYYY. "; }
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
                    objCmd.CommandText = "PR_MIG_CRUD_USUARIO";

                    objCmd.Parameters.Add("NOMBREUSUARIO", OracleDbType.Varchar2).Value = request.Usuario.nombreUsuario;
                    objCmd.Parameters.Add("NOMBRE", OracleDbType.Varchar2).Value = request.Usuario.nombre;
                    objCmd.Parameters.Add("APELLIDO", OracleDbType.Varchar2).Value = request.Usuario.apellido;
                    objCmd.Parameters.Add("CORREOELECTRONICO", OracleDbType.Varchar2).Value = request.Usuario.correoElectronico;
                    objCmd.Parameters.Add("CLAVE", OracleDbType.Varchar2).Value = request.Usuario.clave;
                    objCmd.Parameters.Add("ACTIVO", OracleDbType.Varchar2).Value = request.Usuario.activo;
                    objCmd.Parameters.Add("NOTIFICACIONPROCESO", OracleDbType.Varchar2).Value = request.Usuario.notificacionProceso;
                    objCmd.Parameters.Add("PROCESO", OracleDbType.Varchar2).Value = request.Usuario.proceso;
                    objCmd.Parameters.Add("USUARIOCREACION", OracleDbType.Varchar2).Value = request.Usuario.usuarioCreacion;
                    objCmd.Parameters.Add("FECHACREACION", OracleDbType.Varchar2).Value = request.Usuario.fechaCreacion;
                    objCmd.Parameters.Add("IDCRUD", OracleDbType.Int32).Value = request.Usuario.idcrud;

                    OracleDataReader reader = objCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UsuarioDTO user = new UsuarioDTO();

                        user.nombreUsuario = (string)reader["USUNOMBREUSUARIO"].ToString();
                        user.nombre = (string)reader["USUNOMBRE"].ToString();
                        user.apellido = (string)reader["USUAPELLIDO"].ToString();
                        user.correoElectronico = (string)reader["USUCORREOELECTRONICO"].ToString();
                        user.clave = encrypt.DesencriptarCadenaDeCaracteres((string)reader["USUCLAVE"].ToString(), vector);
                        user.activo = (string)reader["USUACTIVO"].ToString();
                        user.notificacionProceso = (string)reader["USUNOTIFICACIONPROCESO"].ToString();
                        user.proceso = (string)reader["USUPROCESO"].ToString();
                        user.usuarioCreacion = (string)reader["USUUSUARIOCREACION"].ToString();
                        user.fechaCreacion = DBNull.Value.Equals(reader["USUFECHACREACION"]) ? DateTime.Now.ToString() : ((DateTime)reader["USUFECHACREACION"]).ToString();
                        user.idcrud = (decimal)reader["IDCRUD"];
                        user.mensaje = (string)reader["MENSAJE"].ToString();

                        listUsuarios.Add(user);
                    }

                    connection.Dispose();
                }
                catch (Exception ex)
                {
                    string mensaje = ex.ToString();

                    UsuarioDTO userErrorBD = new UsuarioDTO();

                    // Parametros generalizados
                    userErrorBD.nombreUsuario = "";
                    userErrorBD.nombre = "";
                    userErrorBD.apellido = "";
                    userErrorBD.correoElectronico = "";
                    userErrorBD.clave = "";
                    userErrorBD.activo = "";
                    userErrorBD.notificacionProceso = "";
                    userErrorBD.proceso = "";
                    userErrorBD.usuarioCreacion = "";
                    userErrorBD.fechaCreacion = "";

                    // Parametros mensaje advertencia
                    userErrorBD.idcrud = 0;
                    userErrorBD.mensaje = "Error en BD: " + mensaje.Substring(0, 200);

                    listUsuariosAdvertencia.Add(userErrorBD);

                    return listUsuariosAdvertencia;
                }

                return listUsuarios;
            }
            else
            {
                UsuarioDTO userAdvertencia = new UsuarioDTO();

                // Parametros generalizados
                userAdvertencia.nombreUsuario = "";
                userAdvertencia.nombre = "";
                userAdvertencia.apellido = "";
                userAdvertencia.correoElectronico = "";
                userAdvertencia.clave = "";
                userAdvertencia.activo = "";
                userAdvertencia.notificacionProceso = "";
                userAdvertencia.proceso = "";
                userAdvertencia.usuarioCreacion = "";
                userAdvertencia.fechaCreacion = "";

                // Parametros mensaje advertencia
                userAdvertencia.idcrud = request.Usuario.idcrud;
                userAdvertencia.mensaje = "Advertencia: " + mensajeAdvertencia;

                listUsuariosAdvertencia.Add(userAdvertencia);

                return listUsuariosAdvertencia;
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

        public string validarTamanoParametros(UsuarioQuery request)
        {
            // Validar tamaño de parametros:
            if (request.Usuario.nombreUsuario.Length > 100) { mensajeAdvertencia = "El tamaño del parametro nombre usuario no puede ser mayora a 100. "; }
            if (request.Usuario.nombre.Length > 255) { mensajeAdvertencia = "El tamaño del parametro nombre no puede ser mayora a 255. "; }
            if (request.Usuario.apellido.Length > 255) { mensajeAdvertencia = "El tamaño del parametro apellido no puede ser mayora a 255. "; }
            if (request.Usuario.correoElectronico.Length > 255) { mensajeAdvertencia = "El tamaño del parametro correo electronico no puede ser mayora a 255. "; }
            if (request.Usuario.clave.Length > 100) { mensajeAdvertencia = "El tamaño del parametro clave no puede ser mayora a 100. "; }
            if (request.Usuario.activo.Length > 1) { mensajeAdvertencia = "El tamaño del parametro activo no puede ser mayora a 1. "; }
            if (request.Usuario.notificacionProceso.Length > 1) { mensajeAdvertencia = "El tamaño del parametro notificacion proceso no puede ser mayora a 1. "; }
            if (request.Usuario.proceso.Length > 30) { mensajeAdvertencia = "El tamaño del parametro proceso no puede ser mayora a 30. "; }
            if (request.Usuario.usuarioCreacion.Length > 50) { mensajeAdvertencia = "El tamaño del parametro usuario creacion no puede ser mayora a 50. "; }
            if (request.Usuario.fechaCreacion.Length > 10) { mensajeAdvertencia = "El tamaño del parametro fecha creacion no puede ser mayora a 10 (DD/MM/YYYY). "; }

            return mensajeAdvertencia;
        }
    }
}
