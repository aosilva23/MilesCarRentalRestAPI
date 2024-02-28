using Oracle.ManagedDataAccess.Client;
using milescarrental.Application.Configuration.Data;
using milescarrental.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.Helpers
{
    public class AltersSesionFormatoFecha
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        string mensaje;
        public string ejecutarAltersBD()
        {
            mensaje = this.altersSesionFormatoFecha_NLS_TIMESTAMP_FORMAT();
            mensaje += "||" + this.altersSesionFormatoFecha_NLS_TIMESTAMP_TZ_FORMAT();
            mensaje += "||" + this.altersSesionFormatoFecha_NLS_TIME_TZ_FORMAT();
            mensaje += "||" + this.altersSesionFormatoFecha_NLS_DATE_FORMAT();

            return mensaje;
        }

        public string altersSesionFormatoFecha_NLS_TIMESTAMP_FORMAT()
        {
            string mensaje = "";
            try
            {
                var connection = this._sqlConnectionFactory.GetOpenConnection();

                string sql = "ALTER SESSION SET NLS_TIMESTAMP_FORMAT = 'DD/MM/RR HH24:MI:SSXFF'";
                //sql += "SELECT * FROM dates_table";

                OracleCommand objCmd = new OracleCommand(sql);
                objCmd.Connection = (OracleConnection)connection;
                OracleDataReader reader = objCmd.ExecuteReader();

                mensaje = "Exito";
                while (reader.Read())
                {
                    // Read Data
                    mensaje = "Exito";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Fallo: " + ex.ToString();
            }

            return mensaje;
        }

        public string altersSesionFormatoFecha_NLS_TIMESTAMP_TZ_FORMAT()
        {
            string mensaje = "";
            try
            {
                var connection = this._sqlConnectionFactory.GetOpenConnection();

                string sql = "ALTER SESSION SET NLS_TIMESTAMP_TZ_FORMAT = 'DD/MM/RR HH24:MI:SSXFF TZR'";
                //sql += "SELECT * FROM dates_table";

                OracleCommand objCmd = new OracleCommand(sql);
                objCmd.Connection = (OracleConnection)connection;
                OracleDataReader reader = objCmd.ExecuteReader();

                mensaje = "Exito";
                while (reader.Read())
                {
                    // Read Data
                    mensaje = "Exito";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Fallo: " + ex.ToString();
            }

            return mensaje;
        }

        public string altersSesionFormatoFecha_NLS_TIME_TZ_FORMAT()
        {
            string mensaje = "";
            try
            {
                var connection = this._sqlConnectionFactory.GetOpenConnection();

                string sql = "ALTER SESSION SET NLS_TIME_TZ_FORMAT = 'HH24:MI:SSXFF TZR'";
                //sql += "SELECT * FROM dates_table";

                OracleCommand objCmd = new OracleCommand(sql);
                objCmd.Connection = (OracleConnection)connection;
                OracleDataReader reader = objCmd.ExecuteReader();

                mensaje = "Exito";
                while (reader.Read())
                {
                    // Read Data
                    mensaje = "Exito";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Fallo: " + ex.ToString();
            }

            return mensaje;
        }

        public string altersSesionFormatoFecha_NLS_DATE_FORMAT()
        {
            string mensaje = "";
            try
            {
                var connection = this._sqlConnectionFactory.GetOpenConnection();

                string sql = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/RR'";
                //sql += "SELECT * FROM dates_table";

                OracleCommand objCmd = new OracleCommand(sql);
                objCmd.Connection = (OracleConnection)connection;
                OracleDataReader reader = objCmd.ExecuteReader();

                mensaje = "Exito";
                while (reader.Read())
                {
                    // Read Data
                    mensaje = "Exito";
                }
            }
            catch (Exception ex)
            {
                mensaje = "Fallo: " + ex.ToString();
            }

            return mensaje;
        }

    }
}
