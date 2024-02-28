using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.Vehiculo
{
    public class VehiculoDTO
    {
        public decimal id { get; set; }
        public string placa { get; set; }
        public string modelo { get; set; }
        public decimal kilometraje { get; set; }
        public string tipo { get; set; }
        public string nomeclaturamotor { get; set; }
        public string locLatitudVehiculo { get; set; }
        public string locLongitudVehiculo { get; set; }
        public string observaciones { get; set; }
        public string estado { get; set; }
        public string fechaRegistro { get; set; }
        public decimal proceso { get; set; }
        public string usuario { get; set; }
        public string mensaje { get; set; }
    }
}
