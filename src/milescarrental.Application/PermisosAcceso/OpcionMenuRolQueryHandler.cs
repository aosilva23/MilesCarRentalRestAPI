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
    public class OpcionMenuRolQueryHandler : IRequestHandler<OpcionMenuRolQuery, List<OpcionMenuRolDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public OpcionMenuRolQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<OpcionMenuRolDTO>> Handle(OpcionMenuRolQuery request, CancellationToken cancellationToken)
        {
            List<OpcionMenuRolDTO> listUsuariosRoles = new List<OpcionMenuRolDTO>();
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            try
            {
                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = (OracleConnection)connection;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_MIG_CRUD_OPCIONMENUROL";

                objCmd.Parameters.Add("IDROL", OracleDbType.Int32).Value = request.opcionmenuorol.rolId;
                objCmd.Parameters.Add("IDOPCIONMENU", OracleDbType.Varchar2).Value = request.opcionmenuorol.opcionMenuId;
                objCmd.Parameters.Add("IDCRUD", OracleDbType.Int32).Value = request.opcionmenuorol.idcrud;

                // CRUD Usuarios-- > Buscar(1), Insertar(2), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
                decimal opcionCRUD = request.opcionmenuorol.idcrud;

                OracleDataReader reader = objCmd.ExecuteReader();

                while (reader.Read())
                {
                    OpcionMenuRolDTO opcionmenuorol = new OpcionMenuRolDTO();

                    opcionmenuorol.opcionMenuId = Convert.ToInt32(reader["PROIDOPCION"]);
                    opcionmenuorol.rolId = Convert.ToInt32(reader["PROIDROL"]);
                    opcionmenuorol.nombreopcionMenu = (reader["PRONOMBREOPCION"] != DBNull.Value) ? (string)reader["PRONOMBREOPCION"] : "";
                    opcionmenuorol.nombreRol = (reader["PRONOMBREROL"] != DBNull.Value) ? (string)reader["PRONOMBREROL"] : "";
                    opcionmenuorol.idcrud = Convert.ToInt32(reader["IDCRUD"]);
                    opcionmenuorol.mensaje = (string)reader["MENSAJE"].ToString();
                    listUsuariosRoles.Add(opcionmenuorol);
                }

                connection.Dispose();
            }
            catch (Exception ex)
            {
                string mensaje = ex.ToString();
            }

            return listUsuariosRoles;
        }
    }
}
