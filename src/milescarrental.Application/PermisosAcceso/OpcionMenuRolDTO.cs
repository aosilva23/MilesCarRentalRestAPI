using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.PermisosAcceso
{
    public class OpcionMenuRolDTO
    {
        public int rolId { get; set; }

        public string nombreRol { get; set; }
        public int opcionMenuId { get; set; }
        public string nombreopcionMenu { get; set; }
        public int idcrud { get; set; }
        public string mensaje { get; set; }
    }
}
