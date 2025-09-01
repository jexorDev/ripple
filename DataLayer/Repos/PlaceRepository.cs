using Google.Maps.Places.V1;
using Npgsql;
using Ripple.DataLayer.Classes;
using System.Data;
using Newtonsoft.Json;

namespace Ripple.DataLayer.Repos
{
    public class PlaceRepository
    {
        public void Save(Place place, IDbConnection connection)
        {
 
            if(place == null) return;

            const string sql = @"
insert into
    places
(
     id
    ,data
)
values
(
    @id
    ,@data
) 
ON CONFLICT (id) DO NOTHING";

            var itineraryPlaces = new List<ItineraryPlace>();

            using (var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)connection))
            {
                cmd.Parameters.AddWithValue("@id", place.Id);
                cmd.Parameters.AddWithValue("@data", NpgsqlTypes.NpgsqlDbType.Json, Newtonsoft.Json.JsonConvert.SerializeObject(place));

                cmd.ExecuteNonQuery();
            }
        }
    }
}
