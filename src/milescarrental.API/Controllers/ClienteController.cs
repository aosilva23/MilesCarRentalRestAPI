using MediatR;
using Microsoft.AspNetCore.Mvc;
using milescarrental.Application.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using milescarrental.Application.Cliente;

namespace milescarrental.API.Controllers
{
    [ApiController]
    public class ClienteController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppConfiguration _appConfig;
        private readonly IWebHostEnvironment hostEnvironment;

        public ClienteController(IMediator mediator, AppConfiguration appConfig, IWebHostEnvironment hostEnvironment)
        {
            this._mediator = mediator;
            this._appConfig = appConfig;
            this.hostEnvironment = hostEnvironment;
        }

        #region Procesos Clientes --> Buscar (1), Insertar (2), Actualizar (3) y Borrar (4) Registro

        //[Authorize(Roles = "ADMINISTRADOR")]
        //[AllowAnonymous]
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/clientes/procesar-clientes")]
        [ProducesResponseType(typeof(ApiRequestResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(
            Summary = "Administración de Clientes",
            Description = "Endpoint para realizar las tareas sobre la entidad Clientes del aplicativo\n\n" +
                "**Algunos datos importante a tener en cuenta**\n\n" +
                "El atributo **proceso** indica el tipo de operación a realizar, los posibles valores son:\n\n" +
                " 1 => Busqueda Exacta\n\n" +
                " 2 => Insertar\n\n" +
                " 3 => Actualizar\n\n" +
                " 4 => Borrar o Inactivar\n\n" +
                " 5 => Listar todos\n\n" +
                " 6 => Busqueda Genérica\n\n\n" +
            "En los casos que por el tipo de operación no aplique enviar ciertos atributos o no se diponga de ellos, se enviará 0 (cero) para los atributos de tipo numérico y vacío para los atributos tipo string"
        )]
        public async Task<IActionResult> RegisterUser([FromBody] ClienteDTO cliente)
        {
            var apiResponseList = new ApiResponseList<ClienteDTO>();
            ApiRequestResponse apiRequestResponse = new ApiRequestResponse();

            // Validar si el modelo o ClienteDTO es valido:
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var list = await _mediator.Send(new ClienteQuery(cliente));

            if (list.Count() > 0)
            {
                apiResponseList.List = list;
                string mensaje = list[0].mensaje;
                apiRequestResponse.Type = "ListarTest";

                #region Validar el mensaje que se envio:

                if (mensaje.Contains("Error"))
                {
                    apiRequestResponse.Code = 400;
                    apiRequestResponse.Message = "Consulta realizada con Error";
                }

                if (mensaje.Contains("Fallo"))
                {
                    apiRequestResponse.Code = 400;
                    apiRequestResponse.Message = "Consulta realizada con Fallo";
                }

                if (mensaje.Contains("Advertencia"))
                {
                    apiRequestResponse.Code = 400;
                    apiRequestResponse.Message = "Consulta realizada con Advertencia";
                }

                if (mensaje.Contains("Exitoso"))
                {
                    apiRequestResponse.Code = 200;
                    apiRequestResponse.Message = "Consulta realizada con Exito";
                }

                #endregion

                apiResponseList.ApiResponse = apiRequestResponse;

                return Ok(apiResponseList);
            }
            else
            {
                return Conflict(apiResponseList);
            }
        }

        #endregion

    }
}
