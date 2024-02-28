using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.PermisosAcceso
{
    public class RolDTO
    {
        public decimal id { get; set; }
        public string nombre { get; set; }
        public string activo { get; set; }
        public string usuarioCreacion { get; set; }
        public string fechaCreacion { get; set; }
        public decimal idcrud { get; set; }
        public string mensaje { get; set; }
    }
}
 