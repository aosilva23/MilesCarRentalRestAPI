using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using milescarrental.Application.Models;
using milescarrental.Application.Test;

namespace milescarrental.API.Controllers
{
    [ApiController]
    public class TestController : Controller
    {
        private readonly IMediator _mediator;
        private readonly AppConfiguration _appConfig;

        public TestController(IMediator mediator, AppConfiguration appConfig)
        {
            this._mediator = mediator;
            this._appConfig = appConfig;
        }

        /*
        [HttpGet]
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/test/obtener-resultados")]
        [ProducesResponseType(typeof(ApiResponseList<TestGetdata>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTestData()
        {
            var apiResponseList = new ApiResponseList<TestGetdata>();
            var apiRequestResponse = new ApiRequestResponse();
            apiRequestResponse.Code = 200;
            apiRequestResponse.Type = "ListarTest";
            apiRequestResponse.Message = "Consulta realizada con éxito";

            apiResponseList.ApiResponse = apiRequestResponse;
            var list = await _mediator.Send(new TestDataQuery());
            apiResponseList.List = list;


            return Ok(apiResponseList);

        }*/
    }
}
