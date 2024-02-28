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
    public class OpcionesMenuUsuarioQueryHandler : IRequestHandler<OpcionesMenuUsuarioQuery, List<OpcionesMenuUsuarioDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public OpcionesMenuUsuarioQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<OpcionesMenuUsuarioDTO>> Handle(OpcionesMenuUsuarioQuery request, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            List<OpcionesMenuUsuarioDTO> OpcionesMenuUsuarioResponse = new List<OpcionesMenuUsuarioDTO>();
            try
            {
                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = (OracleConnection)connection;
                objCmd.CommandText = "PR_MIG_OPCIONES_MENU_USUARIO";
                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("nombreusuario", OracleDbType.Varchar2).Value = request.nombreUsuario;

                OracleDataReader reader = objCmd.ExecuteReader();

                while (reader.Read())
                {
                    OpcionesMenuUsuarioDTO opcionesMenuUsuario = new OpcionesMenuUsuarioDTO();

                    opcionesMenuUsuario.IdOpcionMenu = Convert.ToInt32(reader["IDOPCIONMENU"]);
                    opcionesMenuUsuario.OpcionMenu = reader["OPCIONMENU"].ToString();
                    opcionesMenuUsuario.Url = reader["OPCIONMENUURL"].ToString();

                    OpcionesMenuUsuarioResponse.Add(opcionesMenuUsuario);
                }

                connection.Dispose();
            }
            catch (Exception e)
            {
                string mensje = e.ToString();

                OpcionesMenuUsuarioResponse.Add(new OpcionesMenuUsuarioDTO()
                {
                    IdOpcionMenu = 0,
                    OpcionMenu = ""
                });

                connection.Dispose();
            }
            return OpcionesMenuUsuarioResponse;
        }
    }
}
