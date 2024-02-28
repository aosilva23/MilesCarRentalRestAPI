using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace milescarrental.Application.PermisosAcceso
{
	public class UsuarioDTO
	{
		//[Required(ErrorMessage = "Se requiere definir un texto para el parametro nombreUsuario")]
		//[MaxLength(10, ErrorMessage = "El nombre de usuario excede el limite de 10 caracteres")]
		public string nombreUsuario { get; set; }

		//[Required(ErrorMessage = "Se requiere definir un texto para el parametro nombre")]
		//[MaxLength(20, ErrorMessage = "El nombre excede el limite de 20 caracteres")]
		public string nombre { get; set; }

		//[Required(ErrorMessage = "Se requiere definir un texto para el parametro apellido")]
		//[MaxLength(20, ErrorMessage = "El apellido excede el limite de 20 caracteres")]
		public string apellido { get; set; }

		public string correoElectronico { get; set; }

		//[Required(ErrorMessage = "Se requiere definir un texto para el parametro clave")]
		public string clave { get; set; }

		//[Required(ErrorMessage = "Se requiere definir S o N para el parametro Activo")]
		//[RegularExpression("[S,N]{1}", ErrorMessage = " El parametro Activo solo puede tomar los valores S o N")]
		public string activo { get; set; }
		
		public string notificacionProceso { get; set; }
		public string proceso { get; set; }
		public string usuarioCreacion { get; set; }
		public string fechaCreacion { get; set; }
		public decimal idcrud { get; set; }
		public string mensaje { get; set; }
	}
}
