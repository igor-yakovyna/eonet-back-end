using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaturalEventsTracker.Services.Interfaces;

namespace NaturalEventsTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NaturalEventsController : ControllerBase
    {
        private readonly IEonetService _eonetService;

        public NaturalEventsController(IEonetService eonetService)
        {
            _eonetService = eonetService ?? throw new ArgumentNullException();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _eonetService.GetAllEvents());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _eonetService.GetEvent(id));
        }
    }
}
