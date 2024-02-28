using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.Alquiler
{
    public class AlquilerQuery : IRequest<List<AlquilerDTO>>
    {
        public AlquilerDTO alquiler { get; set; }
        public AlquilerQuery()
        {

        }

        public AlquilerQuery(AlquilerDTO alquiler)
        {
            this.alquiler = alquiler;
        }
    }
}

