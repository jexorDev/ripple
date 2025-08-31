using Google.Maps.Places.V1;

namespace Ripple.DataLayer.Classes
{
    public class ItineraryPlace
    {
        public int? ItineraryId { get; set; }
        public int Index { get; set; }
        public double VisitHours { get; set; }
        public Place Place { get; set; }
        public long MinutesAwayFromPreviousPlace { get; set; }
        public string Notes { get; set; }
    }
}
