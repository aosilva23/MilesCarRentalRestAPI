using Dapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using milescarrental.Application.Configuration.Data;
using milescarrental.Application.Helpers;
using milescarrental.Application.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace milescarrental.Application.PermisosAcceso
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, List<LoginDTO>>
    {
        private readonly AppConfiguration _appConfig;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public LoginQueryHandler(AppConfiguration appConfig, ISqlConnectionFactory sqlConnectionFactory)
        {
            this._appConfig = appConfig;
            this._sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<LoginDTO>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {            
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            List<LoginDTO> listLogin = new List<LoginDTO>();
            LoginDTO loginDTO = new LoginDTO();

            EncryptorAes encrypt = new EncryptorAes();
            string vector = _appConfig.Vector;

            try
            {
                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = (OracleConnection)connection;
                objCmd.CommandText = "PR_MIG_LOGIN";
                objCmd.CommandType = CommandType.StoredProcedure;

                objCmd.Parameters.Add("nombreusuario", OracleDbType.Varchar2).Value = request.nombreUsuario;
                objCmd.Parameters.Add("claveusuario", OracleDbType.Varchar2).Value = encrypt.EncriptarCadenaDeCaracteres(request.claveUsuario, vector);  ;

                OracleDataReader reader = objCmd.ExecuteReader();

                while (reader.Read())
                {
                    loginDTO.rol = reader["USUROL"].ToString();                    
                }                

                if (loginDTO.rol == "")
                {
                    LoginDTO loginSinToken = new LoginDTO()
                    {
                        nombreUsuario = request.nombreUsuario,
                        clave = "",
                        rol = loginDTO.rol,
                        tiempoToken = _appConfig.TiempoToken,
                        token = "NO TOKEN"
                    };

                    listLogin.Add(loginSinToken);

                    return listLogin;
                }
                else
                {
                    #region Generar el Token con JWT:
                    
                    var manejadorToken = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appConfig.ApiKey);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, request.nombreUsuario),
                            new Claim(ClaimTypes.Role, loginDTO.rol)
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(_appConfig.TiempoToken),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = manejadorToken.CreateToken(tokenDescriptor);
                    
                    #endregion

                    LoginDTO loginToken = new LoginDTO()
                    {
                        nombreUsuario = request.nombreUsuario,
                        clave = "",
                        rol = loginDTO.rol,
                        tiempoToken = _appConfig.TiempoToken,
                        token = manejadorToken.WriteToken(token)
                    };

                    string tokenGenerado = loginToken.token;

                    listLogin.Add(loginToken);

                    connection.Dispose();

                    return listLogin;
                }                
            }
            catch (Exception e)
            {
                string mensje = e.ToString();
                connection.Dispose();

                LoginDTO loginException = new LoginDTO()
                {
                    nombreUsuario = request.nombreUsuario,
                    clave = "",
                    rol = "",
                    tiempoToken = 0,
                    token = "EXCEPTION TOKEN"
                };

                listLogin.Add(loginException);

                return listLogin;              
            }            
        }
    }
}
