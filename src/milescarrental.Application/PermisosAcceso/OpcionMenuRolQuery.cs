using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.PermisosAcceso
{
    public class OpcionMenuRolQuery : IRequest<List<OpcionMenuRolDTO>>
    {        
        public OpcionMenuRolDTO opcionmenuorol { get; set; }
        public OpcionMenuRolQuery()
        {

        }

        public OpcionMenuRolQuery(OpcionMenuRolDTO opcionmenuorol)
        {
            this.opcionmenuorol = opcionmenuorol;
        }
    }
}
