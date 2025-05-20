using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Ripple.DataLayer.Classes;

namespace Ripple.DataLayer.Repos
{
    public class ItineraryRepository
    {
        public async Task<List<Itinerary>> Get(ProtectedLocalStorage protectedLocalStorage)
        {
            var result = await protectedLocalStorage.GetAsync<List<Itinerary>>("itineraries");
            var itineraries = result.Value;
            return itineraries == null ? itineraries = new List<Itinerary>() : itineraries;
        }

        public async Task Save(ProtectedLocalStorage protectedLocalStorage, List<Itinerary> itineraries)
        {
            await protectedLocalStorage.SetAsync("itineraries", itineraries);
        }

        public async Task Save(ProtectedLocalStorage protectedLocalStorage, Itinerary itinerary)
        {
            var itineraries = await Get(protectedLocalStorage);
            var persistedItinerary = itineraries.FirstOrDefault(x => x.Id == itinerary.Id);

            if (persistedItinerary != null)
            {
                itineraries.Remove(persistedItinerary);
                
            }
            itineraries.Add(itinerary);


            await protectedLocalStorage.SetAsync("itineraries", itineraries);
        }
    }
}
