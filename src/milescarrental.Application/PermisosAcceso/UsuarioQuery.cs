using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.PermisosAcceso
{
    public class UsuarioQuery : IRequest<List<UsuarioDTO>>
    {
        public UsuarioDTO Usuario { get; set; }
        public UsuarioQuery()
        {

        }

        public UsuarioQuery(UsuarioDTO usuario)
        {
            this.Usuario = usuario;
        }        
    }
}
