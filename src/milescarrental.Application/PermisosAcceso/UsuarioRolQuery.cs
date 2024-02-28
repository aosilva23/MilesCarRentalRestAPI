using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.PermisosAcceso
{
    public class UsuarioRolQuery : IRequest<List<UsuarioRolDTO>>
    {
        public UsuarioRolDTO usuariorol { get; set; }
        public UsuarioRolQuery()
        {

        }

        public UsuarioRolQuery(UsuarioRolDTO usuariorol)
        {
            this.usuariorol = usuariorol;
        }
    }
}
