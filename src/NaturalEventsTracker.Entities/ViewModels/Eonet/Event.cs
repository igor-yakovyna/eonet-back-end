namespace NaturalEventsTracker.Entities.ViewModels.Eonet
{
    public class Event
    {
        /// <summary>
        /// Unique id for this event.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The title of the event.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Optional longer description of the event. Most likely only a sentence or two.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// The full link to the API endpoint for this specific event.
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// One or more categories assigned to the event.
        /// </summary>
        public Category[] categories { get; set; }

        /// <summary>
        /// One or more sources that refer to more information about the event.
        /// </summary>
        public Source[] sources { get; set; }

        /// <summary>
        /// One or more event geometries are the pairing of a specific date/time with a location.
        /// The date/time will most likely be 00:00Z unless the source provided a particular time. 
        /// The geometry will be a GeoJSON object of either type "Point" or "Polygon."
        /// </summary>
        public Geometric[] geometries { get; set; }

        /// <summary>
        /// An event is deemed “closed” when it has ended. The closed field will include a date/time when the event has ended. 
        /// Depending upon the nature of the event, the closed value may or may not accurately represent the absolute ending of the event.
        /// </summary>
        public string closed { get; set; }
    }
}
