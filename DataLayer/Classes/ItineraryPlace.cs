using Google.Maps.Places.V1;

namespace Ripple.DataLayer.Classes
{
    public class ItineraryPlace
    {
        public int Index { get; set; }
        public double VisitHours { get; set; }
        //public long SecondsAwayFromPreviousPlace { get; set; }
        //public long MinutesAwayFromPreviousPlace => SecondsAwayFromPreviousPlace / 60;
        public Place Place { get; set; }
        public string GoogleMapsUrl { get; set; }
        public long MinutesAwayFromPreviousPlace { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
