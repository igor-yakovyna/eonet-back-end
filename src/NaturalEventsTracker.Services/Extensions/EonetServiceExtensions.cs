using System.Collections.Generic;
using NaturalEventsTracker.Entities.ViewModels.Eonet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace NaturalEventsTracker.Services.Extensions
{
    public static class EonetServiceExtensions
    {
        public static IEnumerable<Event> ReadEvents(this string stringContent)
        {
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Error = HandleDeserializationError
            });

            var events =  JObject.Parse(stringContent)["events"]?.ToObject<IEnumerable<Event>>(jsonSerializer);
            return events;
        }

        public static Event ReadEvent(this string stringContent)
        {
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Error = HandleDeserializationError
            });

            var events = JObject.Parse(stringContent)?.ToObject<Event>(jsonSerializer) ?? null;
            return events;
        }

        public static IEnumerable<Source> ReadSources(this string stringContent)
        {
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Error = HandleDeserializationError
            });

            var sources = JObject.Parse(stringContent)["sources"]?.ToObject<IEnumerable<Source>>(jsonSerializer);
            return sources;
        }

        private static void HandleDeserializationError(object sender, ErrorEventArgs errorEventArgs)
        {
            errorEventArgs.ErrorContext.Handled = true;
        }
    }
}
