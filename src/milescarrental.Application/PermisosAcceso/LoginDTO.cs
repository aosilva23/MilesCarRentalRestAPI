using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.PermisosAcceso
{
    public class LoginDTO
    {
        public string nombreUsuario { get; set; }
        public string clave { get; set; }
        public string rol { get; set; }
        public int tiempoToken { get; set; }
        public string token { get; set; }
    }
}
