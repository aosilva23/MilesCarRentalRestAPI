using System;
using System.Collections.Generic;
using System.Text;

namespace milescarrental.Application.Cliente
{
    public class ClienteDTO
    {
        public decimal id { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string nombreCompleto { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
        public string observaciones { get; set; }
        public string estado { get; set; }
        public string fechaRegistro { get; set; }
        public decimal proceso { get; set; }
        public string usuario { get; set; }
        public string mensaje { get; set; }
    }
}
