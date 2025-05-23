using Npgsql;
using Ripple.DataLayer.Classes;
using System.Data;

namespace Ripple.DataLayer.Repos
{
    public class ItineraryRepository
    {
        public List<Itinerary> Get(int? id, IDbConnection connection)
        {
            const string sql = @"
select
     id
    ,name
    ,start_date
    ,start_time
from
    itineraries
where
    @id is null
or
    id = @id";

            var itineraries = new List<Itinerary>();

            using (var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)connection))
            {
                cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, id.HasValue ? id.Value : DBNull.Value);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itineraries.Add(new Itinerary
                        {
                            Id = reader.GetFieldValue<int>("id"),
                            Name = reader.GetFieldValue<string>("name"),
                            ItineraryTime = reader.GetFieldValue<DateTime>("start_time"),
                            ItineraryDate = reader.GetFieldValue<DateTime>("start_date")
                        });
                    }
                }
            }

            return itineraries;
        }

        public int? Create(Itinerary itinerary, IDbConnection connection)
        {
            const string sql = @"
insert into 
    itineraries
(
     name
    ,start_date
    ,start_time
)
values
(
     @name
    ,@start_date
    ,@start_time
)
RETURNING id";

            var itineraries = new List<Itinerary>();

            using (var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)connection))
            {
                cmd.Parameters.AddWithValue("@name", itinerary.Name);
                cmd.Parameters.AddWithValue("@start_date", itinerary.ItineraryDate);
                cmd.Parameters.AddWithValue("@start_time", itinerary.ItineraryTime);
                cmd.Parameters.Add(new NpgsqlParameter { ParameterName = "@id", DbType = DbType.Int16, Direction = ParameterDirection.Output });

                cmd.ExecuteNonQuery();

                var id = cmd.Parameters["@id"].Value;

                if (id != null)
                {
                    return int.Parse(id.ToString());
                }
                return null;

            }
        }

        public void Update(Itinerary itinerary, IDbConnection connection)
        {
            const string sql = @"
update
    itineraries
set
     name = @name
    ,start_date = @start_date
    ,start_time = @start_time
where
    id = @id";

            var itineraries = new List<Itinerary>();

            using (var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)connection))
            {
                cmd.Parameters.AddWithValue("@id", itinerary.Id);
                cmd.Parameters.AddWithValue("@name", itinerary.Name);
                cmd.Parameters.AddWithValue("@start_date", itinerary.ItineraryDate);
                cmd.Parameters.AddWithValue("@start_time", itinerary.ItineraryTime);

                cmd.ExecuteNonQuery();                

            }
        }
    }
}
