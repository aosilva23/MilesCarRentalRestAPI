using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.PermisosAcceso
{
    public class RolQuery : IRequest<List<RolDTO>>
    {
        public RolDTO rol { get; set; }
        public RolQuery()
        {

        }

        public RolQuery(RolDTO rol)
        {
            this.rol = rol;
        }        
    }
}
