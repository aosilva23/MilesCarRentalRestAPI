using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.Cliente
{
    public class ClienteQuery : IRequest<List<ClienteDTO>>
    {
        public ClienteDTO cliente { get; set; }
        public ClienteQuery()
        {

        }

        public ClienteQuery(ClienteDTO cliente)
        {
            this.cliente = cliente;
        }
    }
}
