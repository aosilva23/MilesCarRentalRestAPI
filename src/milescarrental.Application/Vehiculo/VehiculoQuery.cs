using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace milescarrental.Application.Vehiculo
{
    public class VehiculoQuery : IRequest<List<VehiculoDTO>>
    {
        public VehiculoDTO vehiculo { get; set; }
        public VehiculoQuery()
        {

        }

        public VehiculoQuery(VehiculoDTO vehiculo)
        {
            this.vehiculo = vehiculo;
        }
    }
}
