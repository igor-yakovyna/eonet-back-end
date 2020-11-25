using System.Collections.Generic;
using System.Threading.Tasks;
using NaturalEventsTracker.Entities.ViewModels;

namespace NaturalEventsTracker.Services.Interfaces
{
    public interface IEonetService
    {
        Task<IEnumerable<EventViewModel>> GetAllEvents();
        Task<EventViewModel> GetEvent(string id);

    }
}
