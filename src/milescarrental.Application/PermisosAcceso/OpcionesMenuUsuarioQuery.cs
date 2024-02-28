using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.PermisosAcceso
{
    public class OpcionesMenuUsuarioQuery : IRequest<List<OpcionesMenuUsuarioDTO>>
    {
        public OpcionesMenuUsuarioDTO opcionesMenuUsuario { get; set; }

        public string nombreUsuario { get; set; }
        public OpcionesMenuUsuarioQuery(){}

        public OpcionesMenuUsuarioQuery(string nombreUsuario)
        {
            this.nombreUsuario = nombreUsuario;
        }

        public OpcionesMenuUsuarioQuery(OpcionesMenuUsuarioDTO opcionesMenuUsuario)
        {
            this.opcionesMenuUsuario = opcionesMenuUsuario;
        }
    }
}
