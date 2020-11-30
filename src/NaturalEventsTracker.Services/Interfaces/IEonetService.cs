using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalEventsTracker.Entities.ViewModels;

namespace NaturalEventsTracker.Services.Interfaces
{
    public interface IEonetService
    {
        Task<IEnumerable<EventViewModel>> GetAllEventsAsync();
        Task<EventViewModel> GetEventAsync(string id);
        Task<IEnumerable<EventViewModel>> GetFilteredEventsAsync(string sources = null, string status = null, int? days = null);
        Task<IEnumerable<SourceViewModel>> GetEventsSources();
    }
}
