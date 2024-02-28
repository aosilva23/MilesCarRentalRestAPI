using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.Alquiler
{
    public class AlquilerDTO
    {
        public decimal id { get; set; }
        public decimal idCliente { get; set; }
        public decimal idVehiculo { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public decimal kilometrajeInicial { get; set; }
        public decimal kilometrajeFinal { get; set; }
        public string locLatitudVehiculo { get; set; }
        public string locLongitudVehiculo { get; set; }
        public string locLatitudCliente { get; set; }
        public string locLongitudCliente { get; set; }
        public decimal costoAlquiler { get; set; }
        public string estado { get; set; }
        public string observaciones { get; set; }
        public string fechaRegistro { get; set; }
        public decimal proceso { get; set; }
        public string usuario { get; set; }
        public string mensaje { get; set; }
    }
}
