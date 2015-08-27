using System;
using Aggregator.Objects.Configuration;

namespace Aggregator.Objects.Search.Hotels
{
    public class AccommodationSearchLog
    {
        public string Tracker { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public int NumberOfInfants { get; set; }
        public string ChildAges { get; set; }
        public int? MinStarRating { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public long CountryCode { get; set; }
        public long CityCode { get; set; }
        public long ResortCode { get; set; }
        public string SuperIds { get; set; }
        public DateTime StartOfSearch { get; set; }
        public int TimeToSearchSupplier { get; set; }
        public int TimeToSearchTotal { get; set; }
        public int NumberOfHotelResults { get; set; }
        public string Error { get; set; }

        public void ConvertSearch(AccommodationSearchRequest request, AggregatorSetting aggregatorSetting)
        {
            Tracker = aggregatorSetting.Tracker;
            NumberOfRooms = request.NumberOfRooms;
            NumberOfAdults = request.NumberOfAdults;
            NumberOfChildren = request.ChildAge == null ? 0 : request.ChildAge.Count;
            NumberOfInfants = request.NumberOfInfants ?? 0;
            if (request.ChildAge != null)
            {
                var isFirst = true;
                foreach (var childAge in request.ChildAge)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        ChildAges += ",";

                    ChildAges += childAge;
                }
            }
            MinStarRating = request.MinStarRating;
            CheckIn = request.CheckIn;
            CheckOut = request.CheckOut;
            CountryCode = request.CountryCode;
            CityCode = request.CityCode;
            ResortCode = request.ResortCode ?? 0;

            if (request.SuperIds != null && request.SuperIds.Count != 0)
            {
                var isFirst = true;
                foreach (var superId in request.SuperIds)
                {
                    if (isFirst)
                        isFirst = false;
                    else
                        SuperIds += ",";

                    SuperIds += superId;
                }
            }
        }
    }
}