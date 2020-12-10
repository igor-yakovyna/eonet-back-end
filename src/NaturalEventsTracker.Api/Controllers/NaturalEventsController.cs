using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaturalEventsTracker.Entities.ViewModels;
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

        [HttpGet("filter/{sources?}/{status?}/{days?}")]
        public async Task<IActionResult> Get(string sources = null, string status = null, int? days = null)
        {
            return Ok(await _eonetService.GetFilteredEvents(new FiltersViewModel {Sources = sources, Status = status, Days = days }));
        }

        [HttpGet("sources")]
        public async Task<IActionResult> GetSources()
        {
            return Ok(await _eonetService.GetEventsSources());
        }
    }
}
