using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.PermisosAcceso
{
    public class LoginQuery : IRequest<List<LoginDTO>>
    {
        public LoginDTO login { get; set; }
        public string nombreUsuario { get; set; }
        public string claveUsuario { get; set; }        

        public LoginQuery(){}

        public LoginQuery(string nombre, string clave)
        {
            this.nombreUsuario = nombre;
            this.claveUsuario = clave;
        }
        public LoginQuery(LoginDTO login)
        {
            this.login = login;
        }
    }
}
