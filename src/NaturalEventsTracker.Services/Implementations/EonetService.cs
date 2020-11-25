using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaturalEventsTracker.Entities.AppSettingsModels;
using NaturalEventsTracker.Entities.ViewModels;
using NaturalEventsTracker.Entities.ViewModels.Eonet;
using NaturalEventsTracker.Services.Extensions;
using NaturalEventsTracker.Services.Interfaces;
using AutoMapper;

namespace NaturalEventsTracker.Services.Implementations
{
    public class EonetService : IEonetService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EonetService> _logger;
        private readonly IMapper _mapper;
        private readonly EonetSettings _eonetSettings;

        public EonetService(IHttpClientFactory httpClientFactory,
            ILogger<EonetService> logger,
            IMapper mapper,
            IOptions<AppSettings> option)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
            _eonetSettings = option?.Value.EonetSettings ?? throw new ArgumentNullException();
        }

        public async Task<IEnumerable<EventViewModel>> GetAllEvents()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync($"https://{_eonetSettings.Host}/api/{_eonetSettings.ApiVersion}/events");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var events = responseContent.ReadEvents();
                
                var eventsViewModel = _mapper.Map<IEnumerable<Event>, IEnumerable<EventViewModel>>(events);

                return eventsViewModel;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Unable to retrieve EONET Events.";
                _logger.LogError(ex, errorMessage);
                throw ex;
            }
        }

        public Task<EventViewModel> GetEvent(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
