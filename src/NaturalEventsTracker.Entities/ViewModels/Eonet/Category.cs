namespace NaturalEventsTracker.Entities.ViewModels.Eonet
{
    public class Category
    {
        /// <summary>
        /// Unique id for this category.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// The title of the category.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Longer description of the category, addressing the scope. Most likely only a sentence or two.
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// The full link to the API endpoint for this specific category, 
        /// which is the same as the Categories API endpoint filtered to return only events from this category.
        /// </summary>
        public string link { get; set; }

        /// <summary>
        /// A service endpoint that points to the Layers API endpoint filtered to return only layers from this category.
        /// </summary>
        public string layers { get; set; }
    }
}
