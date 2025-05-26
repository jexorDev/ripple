using Ripple.DataLayer.Classes;
using Ripple.DataLayer.Repos;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace Ripple.DataLayer.Models
{
    public class ItineraryModel : INotifyPropertyChanged
    {
        private Itinerary _dto;
        private bool _isNew;

        public ItineraryModel(Itinerary dto)
        {
            _isNew = false;
            _dto = dto;
        }

        public ItineraryModel()
        {
            _isNew = true;
            _dto = new Itinerary();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int? Id
        {
            get => _dto.Id;
            set => _dto.Id = value;
        }

        public string Name
        {
            get => _dto.Name;
            set => _dto.Name = value;

        }

        public DateTime? ItineraryDate
        {
            get => _dto?.ItineraryDate;
            set
            {
                if (value != _dto.ItineraryDate)
                {
                    _dto.ItineraryDate = value;
                    NotifyPropertyChanged(nameof(ItineraryDate));
                }
            }
        }

        public DateTime? ItineraryTime
        {
            get => _dto?.ItineraryTime;
            set
            {
                if (value != _dto.ItineraryTime)
                {
                    _dto.ItineraryTime = value;
                    NotifyPropertyChanged(nameof(ItineraryTime));
                }
            }
        }

        public void Save(ItineraryRepository repo, IDbConnection connection)
        {
            if (_isNew)
            {
                Id = repo.Create(_dto, connection);
            }
            else
            {
                repo.Update(_dto, connection);
            }
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
