using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using milescarrental.Application.Models;
using milescarrental.Application.PermisosAcceso;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace milescarrental.API.Controllers
{
    [ApiController]
    public class PermisosAcesoController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppConfiguration _appConfig;
        private string claveSecreta;

        public PermisosAcesoController(IMediator mediator, AppConfiguration appConfig)
        {
            this._mediator = mediator;
            this._appConfig = appConfig;
            this.claveSecreta = appConfig.ApiKey;
        }

        #region CRUD Usuarios --> Buscar (1), Insertar (2), Actualizar (3) y Borrar (4) Registro

        //[Authorize(Roles = "ADMINISTRADOR")]
        //[AllowAnonymous]
        [HttpPost]
        [ApiVersion("1.0")]        
        [Route("api/v{version:apiVersion}/permisosacceso/crud-usuario")]
        [ProducesResponseType(typeof(ApiRequestResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(
            Summary = "Administración de Usuarios",
            Description = "Endpoint para realizar las tareas administrativas CRUD de Usuarios del aplicativo\n\n" +
                "**Algunos datos importante a tener en cuenta**\n\n" +
                "El atributo **idCrud** indica el tipo de operación a realizar, los posibles valores son:\n\n" +
                " 1 => Busqueda Exacta\n\n" +
                " 2 => Insertar\n\n" +
                " 3 => Actualizar\n\n" +
                " 4 => Borrar o Inactivar\n\n" +
                " 5 => Listar todos\n\n" +
                " 6 => Busqueda Genérica\n\n\n" +
            "En los casos que por el tipo de operación no aplique enviar ciertos atributos o no se diponga de ellos, se enviará 0 (cero) para los atributos de tipo numérico y vacío para los atributos tipo string"
        )]
        public async Task<IActionResult> RegisterUser([FromBody] UsuarioDTO usuario)
        {
            var apiResponseList = new ApiResponseList<UsuarioDTO>();
            ApiRequestResponse apiRequestResponse = new ApiRequestResponse();

            // Validar si el modelo o UsuarioDTO es valido:
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var list = await _mediator.Send(new UsuarioQuery(usuario));

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

        #region CRUD Roles --> Buscar (1), Insertar (2), Actualizar (3) y Borrar (4) Registro

        //[Authorize(Roles = "ADMINISTRADOR")]
        //[AllowAnonymous]
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/permisosacceso/crud-rol")]
        [ProducesResponseType(typeof(ApiRequestResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(
            Summary = "Administración de Roles",
            Description = "Endpoint para realizar las tareas administrativas CRUD de Roles del aplicativo\n\n" +
                "**Algunos datos importante a tener en cuenta**\n\n" +
                "El atributo **idCrud** indica el tipo de operación a realizar, los posibles valores son:\n\n" +
                " 1 => Busqueda Exacta\n\n" +
                " 2 => Insertar\n\n" +
                " 3 => Actualizar\n\n" +
                " 4 => Borrar o Inactivar\n\n" +
                " 5 => Listar todos\n\n" +
                " 6 => Busqueda Genérica\n\n\n" +
            "En los casos que por el tipo de operación no aplique enviar ciertos atributos o no se diponga de ellos, se enviará 0 (cero) para los atributos de tipo numérico y vacío para los atributos tipo string"
        )]
        public async Task<IActionResult> RegisterRol([FromBody] RolDTO rol)
        {
            var apiResponseList = new ApiResponseList<RolDTO>();
            ApiRequestResponse apiRequestResponse = new ApiRequestResponse();

            var list = await _mediator.Send(new RolQuery(rol));

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

        #region CRUD Opciones Menu --> Buscar (1), Insertar (2), Actualizar (3) y Borrar (4) Registro

        //[Authorize(Roles = "ADMINISTRADOR")]
        //[AllowAnonymous]
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/permisosacceso/crud-opcionmenu")]
        [ProducesResponseType(typeof(ApiRequestResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(
            Summary = "Administración de Opciones de Menú",
            Description = "Endpoint para realizar las tareas administrativas CRUD de Menus o Opciones de Menu del aplicativo\n\n" +
                "**Algunos datos importante a tener en cuenta**\n\n" +
                "El atributo **idCrud** indica el tipo de operación a realizar, los posibles valores son:\n\n" +
                " 1 => Busqueda Exacta\n\n" +
                " 2 => Insertar\n\n" +
                " 3 => Actualizar\n\n" +
                " 4 => Borrar\n\n" +
                " 5 => Listar todos\n\n" +
                " 6 => Busqueda Genérica\n\n\n" +
            "En los casos que por el tipo de operación no aplique enviar ciertos atributos o no se diponga de ellos, se enviará 0 (cero) para los atributos de tipo numérico y vacío para los atributos tipo string"
        )]
        public async Task<IActionResult> RegisterOpcionMenu([FromBody] OpcionMenuDTO opcionMenu)
        {
            var apiResponseList = new ApiResponseList<OpcionMenuDTO>();
            ApiRequestResponse apiRequestResponse = new ApiRequestResponse();

            var list = await _mediator.Send(new OpcionMenuQuery(opcionMenu));

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

        #region CRUD UsuarioRol --> Buscar(1), Insertar(2), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)

        //[Authorize(Roles = "ADMINISTRADOR")]
        //[AllowAnonymous]
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/permisosacceso/crud-usuariorol")]
        [ProducesResponseType(typeof(ApiRequestResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(
            Summary = "Administración de Usuarios - Roles",
            Description = "Endpoint para realizar las tareas administrativas CRUD de Usuarios y Roles del aplicativo\n\n" +
                "**Algunos datos importante a tener en cuenta**\n\n" +
                "El atributo **idCrud** indica el tipo de operación a realizar, los posibles valores son:\n\n" +
                " 1 => Busqueda Exacta\n\n" +
                " 2 => Insertar\n\n" +
                " 4 => Borrar\n\n" +
                " 5 => Listar todos\n\n" +
                " 6 => Busqueda Genérica\n\n\n" +
            "En los casos que por el tipo de operación no aplique enviar ciertos atributos o no se diponga de ellos, se enviará 0 (cero) para los atributos de tipo numérico y vacío para los atributos tipo string"
        )]
        public async Task<IActionResult> RegisterUsuarioRol([FromBody] UsuarioRolDTO usuariorol)
        {
            var apiResponseList = new ApiResponseList<UsuarioRolDTO>();
            ApiRequestResponse apiRequestResponse = new ApiRequestResponse();

            var list = await _mediator.Send(new UsuarioRolQuery(usuariorol));

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

        #region CRUD OpcionMenuRol --> Buscar(1), Insertar(2), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)

        //[Authorize(Roles = "ADMINISTRADOR")]
        //[AllowAnonymous]
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/permisosacceso/crud-opcionmenurol")]
        [ProducesResponseType(typeof(ApiRequestResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(
            Summary = "Administración de Opciones Menú - Roles",
            Description = "Endpoint para realizar las tareas administrativas CRUD de Opciones de Menu y Roles del aplicativo\n\n" +
                "**Algunos datos importante a tener en cuenta**\n\n" +
                "El atributo **idCrud** indica el tipo de operación a realizar, los posibles valores son:\n\n" +
                " 1 => Busqueda Exacta\n\n" +
                " 2 => Insertar\n\n" +
                " 4 => Borrar\n\n" +
                " 5 => Listar todos\n\n" +
                " 6 => Busqueda Genérica\n\n\n" +
            "En los casos que por el tipo de operación no aplique enviar ciertos atributos o no se diponga de ellos, se enviará 0 (cero) para los atributos de tipo numérico y vacío para los atributos tipo string"
        )]
        public async Task<IActionResult> RegisterOpcionMenuRol([FromBody] OpcionMenuRolDTO opcionmenuorol)
        {
            var apiResponseList = new ApiResponseList<OpcionMenuRolDTO>();
            ApiRequestResponse apiRequestResponse = new ApiRequestResponse();

            var list = await _mediator.Send(new OpcionMenuRolQuery(opcionmenuorol));

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

        #region Listar Opciones Menu Usuario 

        [AllowAnonymous]
        [HttpGet]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/permisosacceso/opciones-menu-usuario")]
        [ProducesResponseType(typeof(ApiRequestResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(
            Summary = "Listado opciones menu habilitadas para el usuario ",
            Description = "Endpoint para realizar las tareas listar las opciones de menu por usuario de la aplicativo\n\n" +
                "**Algunos datos importante a tener en cuenta**\n\n" +
                "El atributo **nombreusuario** indica el usuario al cual se le hara la consulta de las opciones de menu que tiene asignadas\n\n" +
                "En los casos que por el tipo de operación no aplique enviar ciertos atributos o no se diponga de ellos, se enviará 0 (cero) para los atributos de tipo numérico y vacío para los atributos tipo string"
        )]
        public async Task<IActionResult> OpcionesMenuUsuario(string nombreUsuario)
        {
            var apiResponseList = new ApiResponseList<OpcionesMenuUsuarioDTO>();
            ApiRequestResponse apiRequestResponse = new ApiRequestResponse();

            var list = await _mediator.Send(new OpcionesMenuUsuarioQuery(nombreUsuario));

            if (list.Count() > 0)
            {
                apiResponseList.List = list;
                apiRequestResponse.Code = 200;
                apiRequestResponse.Type = "ListarTest";

                if (list[0].OpcionMenu == "")
                {
                    apiRequestResponse.Message = "Advetencia: Consulta sin registros";
                }
                else
                {
                    apiRequestResponse.Message = "Exito: Consulta con registros";
                }

                apiResponseList.ApiResponse = apiRequestResponse;

                return Ok(apiResponseList);
            }
            else
            {
                return Conflict(apiResponseList);
            }
        }

        #endregion

        #region Servicio de Autenticacion:

        [AllowAnonymous]
        [HttpPost]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/permisosacceso/Autenticacion")]
        [ProducesResponseType(typeof(ApiRequestResponse), (int)HttpStatusCode.OK)]
        [SwaggerOperation(
            Summary = "Generar el Token para acceso a la API si el rol del usuario es ADMINISTRADOR",
            Description = "Endpoint para realizar la tarea de generar el Token de la aplicativo\n\n" +
                "**Algunos datos importante a tener en cuenta**\n\n" +
            "En los casos que por el tipo de operación no aplique enviar ciertos atributos o no se diponga de ellos, se enviará vacío para los atributos tipo string"
        )]
        public async Task<IActionResult> Autenticacion(string nombreUsuario, string claveUsuario)
        {
            var apiResponseList = new ApiResponseList<LoginDTO>();
            ApiRequestResponse apiRequestResponse = new ApiRequestResponse();

            var list = await _mediator.Send(new LoginQuery(nombreUsuario, claveUsuario));

            if (list.Count() > 0)
            {
                if(list[0].token != "NO TOKEN")
                {
                    if (list[0].rol == "")
                    {
                        return Conflict(apiResponseList);
                    }

                    List<LoginDTO> listLogin = new List<LoginDTO>();

                    LoginDTO login = new LoginDTO();
                    login.nombreUsuario = list[0].nombreUsuario;
                    login.clave = list[0].clave;
                    login.rol = list[0].rol;
                    login.tiempoToken = list[0].tiempoToken;
                    login.token = list[0].token;


                    listLogin.Add(login);

                    apiResponseList.List = listLogin;

                    apiRequestResponse.Code = 200;
                    apiRequestResponse.Type = "ListarTest";
                    apiRequestResponse.Message = "Consulta realizada con éxito";
                    apiResponseList.ApiResponse = apiRequestResponse;
                }
                else
                {
                    List<LoginDTO> listLogin = new List<LoginDTO>();

                    LoginDTO login = new LoginDTO();
                    login.nombreUsuario = "";
                    login.clave = "";
                    login.rol = "";
                    login.tiempoToken = 0;
                    login.token = "";


                    listLogin.Add(login);

                    apiResponseList.List = listLogin;

                    apiRequestResponse.Code = 200;
                    apiRequestResponse.Type = "ListarTest";
                    apiRequestResponse.Message = "Nombre de usuario o clave incorrectos";
                    apiResponseList.ApiResponse = apiRequestResponse;
                }

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
