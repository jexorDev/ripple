using Google.Maps.Places.V1;
using Newtonsoft.Json;
using Npgsql;
using Ripple.DataLayer.Classes;
using System.Data;

namespace Ripple.DataLayer.Repos
{
    public class ItineraryPlacesRepository
    {
        public List<ItineraryPlace> Get(int itineraryId, IDbConnection connection)
        {
            const string sql = @"
select
     itinerary_id
    ,place_id
    ,sequence
    ,visit_hours
    ,commute_minutes
    ,p.data
from
    itinerary_places ip
inner join
    places p
on
    ip.place_id = p.id
where
    itinerary_id = @itinerary_id";

            var itineraryPlaces = new List<ItineraryPlace>();

            using (var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)connection))
            {
                cmd.Parameters.AddWithValue("@itinerary_id", itineraryId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var placeJson = reader.GetFieldValue<string>("Data");
                        itineraryPlaces.Add(new ItineraryPlace
                        {
                            Index = reader.GetFieldValue<int>("sequence"),
                            VisitHours = reader.GetFieldValue<double>("visit_hours"),
                            MinutesAwayFromPreviousPlace = reader.GetFieldValue<long>("commute_minutes"),
                            Place = JsonConvert.DeserializeObject<Place>(placeJson)
                        });
                    }
                }
            }

            return itineraryPlaces;
        }

        public void Save(ItineraryPlace itineraryPlace, int itineraryId, IDbConnection connection)
        {
            const string sql = @"
insert into
    itinerary_places
(
     itinerary_id
    ,place_id
    ,sequence
    ,visit_hours
    ,commute_minutes
)
values
(
    @itinerary_id
    ,@place_id
    ,@sequence
    ,@visit_hours
    ,@commute_minutes
) 
";

            var itineraryPlaces = new List<ItineraryPlace>();

            using (var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)connection))
            {
                cmd.Parameters.AddWithValue("@itinerary_id", itineraryId);
                cmd.Parameters.AddWithValue("@place_id", itineraryPlace.Place.Id);
                cmd.Parameters.AddWithValue("@sequence", itineraryPlace.Index);
                cmd.Parameters.AddWithValue("@visit_hours", itineraryPlace.VisitHours);
                cmd.Parameters.AddWithValue("@commute_minutes", itineraryPlace.MinutesAwayFromPreviousPlace);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteAllPlaces(int itineraryId, IDbConnection connection)
        {
            const string sql = @"
delete from
    itinerary_places
where 
    itinerary_id = @itinerary_id";

            var itineraryPlaces = new List<ItineraryPlace>();

            using (var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)connection))
            {
                cmd.Parameters.AddWithValue("@itinerary_id", itineraryId);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
