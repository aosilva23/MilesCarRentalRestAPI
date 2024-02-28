using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.PermisosAcceso
{
    public class PermisosRolOpcionDTO
    {
        public decimal rolId { get; set; }
        public decimal opcionId { get; set; }
        public decimal idcrud { get; set; }
        public string mensaje { get; set; }
    }
}
