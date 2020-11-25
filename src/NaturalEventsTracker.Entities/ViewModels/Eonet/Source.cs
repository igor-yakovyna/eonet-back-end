namespace NaturalEventsTracker.Entities.ViewModels.Eonet
{
    public class Source
    {
        /// <summary>
        /// Unique id for this type.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The title of this source.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///  The homepage URL for the source.
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// The full link to the API endpoint for this specific source, 
        /// which is the same as the Events API endpoint only filtered to return only events from this source.
        /// </summary>
        public string link { get; set; }
    }
}
