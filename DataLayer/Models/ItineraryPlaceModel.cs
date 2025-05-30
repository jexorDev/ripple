using Google.Maps.Places.V1;
using Ripple.DataLayer.Classes;
using Ripple.DataLayer.Repos;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace Ripple.DataLayer.Models
{
    public class ItineraryPlaceModel : INotifyPropertyChanged
    {
        private ItineraryPlace _dto;

        public ItineraryPlaceModel(ItineraryPlace dto)
        {
            _dto = dto;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public double VisitHours
        {
            get => _dto.VisitHours;
            set
            {
                if (value !=  _dto.VisitHours)
                {
                    _dto.VisitHours = value;
                    NotifyPropertyChanged(nameof(VisitHours));
                }
            }
        }

        public long CommuteMinutes
        {
            get => _dto.MinutesAwayFromPreviousPlace;
            set
            {
                if (value != _dto.MinutesAwayFromPreviousPlace)
                {
                    _dto.MinutesAwayFromPreviousPlace = value;
                    NotifyPropertyChanged(nameof(CommuteMinutes));
                }
            }
        }

        public int Index
        {
            get => _dto.Index;
            set => _dto.Index = value;
        }

        public Place Place
        {
            get => _dto.Place;
            set => _dto.Place = value;
        }


        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public string GoogleMapsDirectionsUrl { get; set; } = string.Empty;
        public string GoogleMapsPlaceUrl { get; set; } = string.Empty;

        public void Save(int itineraryId, ItineraryPlacesRepository repo, IDbConnection connection)
        {
            repo.Save(_dto, itineraryId, connection);
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
