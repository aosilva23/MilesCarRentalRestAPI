using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.PermisosAcceso
{
    public class OpcionMenuQuery : IRequest<List<OpcionMenuDTO>>
    {
        public OpcionMenuDTO opcionMenu { get; set; }
        public OpcionMenuQuery()
        {

        }

        public OpcionMenuQuery(OpcionMenuDTO opcionMenu)
        {
            this.opcionMenu = opcionMenu;
        }
    }
}
