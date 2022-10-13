using EnvioFacturaSMS.Applications.CQRS.Abono;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EnvioFacturaSMS.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AbonoController : Controller
    {
        private readonly IMediator _mediator;
        public AbonoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EncolarAbonoQuery SmsAbono)
        {
            await _mediator.Send(SmsAbono);
            return Ok();
        }
    }
}
