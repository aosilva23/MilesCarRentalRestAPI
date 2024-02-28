using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.PermisosAcceso
{
    public class UsuarioRolDTO
    {
        public int rolId { get; set; }
        public string nombreRol { get; set; }
        public string nombreUsuario { get; set; }
        public int idcrud { get; set; }
        public string mensaje { get; set; }
    }
}
