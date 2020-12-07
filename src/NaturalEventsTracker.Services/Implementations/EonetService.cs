using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NaturalEventsTracker.Entities.AppSettingsModels;
using NaturalEventsTracker.Entities.ViewModels;
using NaturalEventsTracker.Entities.ViewModels.Eonet;
using NaturalEventsTracker.Services.Extensions;
using NaturalEventsTracker.Services.Interfaces;
using AutoMapper;
using NaturalEventsTracker.Entities.Constants;

namespace NaturalEventsTracker.Services.Implementations
{
    public class EonetService : IEonetService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<EonetService> _logger;
        private readonly IMapper _mapper;
        private readonly EonetSettings _eonetSettings;

        public EonetService(IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache,
            ILogger<EonetService> logger,
            IMapper mapper,
            IOptions<AppSettings> option)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException();
            _memoryCache = memoryCache ?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
            _eonetSettings = option?.Value.EonetSettings ?? throw new ArgumentNullException();
        }

        public async Task<IEnumerable<EventViewModel>> GetAllEvents()
        {
            try
            {
                IEnumerable<EventViewModel> allEvents = null;

                if (!_memoryCache.TryGetValue(CacheKeys.MultipleNaturalEvents, out allEvents))
                {
                    var openEvents = await GetFilteredEvents(status: "open");
                    var closedEvents = await GetFilteredEvents(status: "closed");

                    allEvents = openEvents.Union(closedEvents).OrderBy(e => e.Id);

                    _memoryCache.Set(CacheKeys.MultipleNaturalEvents, allEvents, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
                }

                return allEvents;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Unable to retrieve EONET Events.";
                _logger.LogError(ex, errorMessage);
                throw ex;
            }
        }

        public async Task<EventViewModel> GetEvent(string id)
        {
            try
            {
                EventViewModel eventViewModel = null;

                if (!_memoryCache.TryGetValue(string.Concat(id, CacheKeys.SingleNaturalEvent), out eventViewModel))
                {
                    var httpClient = _httpClientFactory.CreateClient();

                    var response = await httpClient.GetAsync($"https://{_eonetSettings.Host}/api/{_eonetSettings.ApiVersion}/events/{id}");
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var events = responseContent.ReadEvent();

                    eventViewModel = _mapper.Map<Event, EventViewModel>(events);

                    _memoryCache.Set(string.Concat(id, CacheKeys.SingleNaturalEvent), eventViewModel, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
                }
                
                return eventViewModel;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Unable to retrieve EONET Event.";
                _logger.LogError(ex, errorMessage);
                throw ex;
            }
        }

        public async Task<IEnumerable<EventViewModel>> GetFilteredEvents(string sources = null, string status = null, int? days = null)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync($"https://{_eonetSettings.Host}/api/{_eonetSettings.ApiVersion}/events{BuildQueryStringParams(sources, status, days)}");
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

        public async Task<IEnumerable<SourceViewModel>> GetEventsSources()
        {
            try
            {
                IEnumerable<SourceViewModel> sourcesViewModel = null;

                if (!_memoryCache.TryGetValue(CacheKeys.EventsSources, out sourcesViewModel))
                {
                    var httpClient = _httpClientFactory.CreateClient();

                    var response = await httpClient.GetAsync($"https://{_eonetSettings.Host}/api/{_eonetSettings.ApiVersion}/sources");
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var sources = responseContent.ReadSources();

                    sourcesViewModel = _mapper.Map<IEnumerable<Source>, IEnumerable<SourceViewModel>>(sources);

                    _memoryCache.Set(CacheKeys.EventsSources, sourcesViewModel, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1)));
                }

                return sourcesViewModel;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Unable to retrieve EONET Events.";
                _logger.LogError(ex, errorMessage);
                throw ex;
            }
        }

        private static string BuildQueryStringParams(string sources = null, string status = null, int? days = null)
        {
            var queryString = string.Empty;

            if (!string.IsNullOrEmpty(sources))
            {
                queryString = ConcatQueryParam(queryString, "source", sources);
            }

            if (!string.IsNullOrEmpty(status))
            {
                queryString = ConcatQueryParam(queryString, "status", status);
            }

            if (days != null && days > 0)
            {
                queryString = ConcatQueryParam(queryString, "days", days.ToString());
            }

            return queryString;
        }

        private static string ConcatQueryParam(string queryString, string paramName, string paramValue)
        {
            return string.IsNullOrEmpty(queryString) ? string.Concat(queryString, $"?{paramName}={paramValue}") : string.Concat(queryString, $"&{paramName}={paramValue}");
        }
    }
}
