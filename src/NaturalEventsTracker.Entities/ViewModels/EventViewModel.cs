using System.Collections.Generic;

namespace NaturalEventsTracker.Entities.ViewModels
{
    public class EventViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Descrition { get; set; }
        public bool IsClosed { get; set; }
        public IEnumerable<EventSourceViewModel> Sources { get; set; }
        public IEnumerable<GeometriesViewModel> Geometries { get; set; }
    }
}
