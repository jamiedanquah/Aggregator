using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Aggregator.Objects.Search.Hotels.TripAdvisor
{

    // ReSharper disable InconsistentNaming
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class TripAdvisorResponse
    {
        public int api_version { get; set; }
        public List<int> hotel_ids { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public int num_adults { get; set; }
        public int num_rooms { get; set; }
        public string currency { get; set; }
        public string user_country { get; set; }
        public string device_type { get; set; }
        public string query_key { get; set; }
        public string lang { get; set; }
        public int num_hotels { get; set; }
        public List<TripAdvisorResponseHotel> hotels { get; set; }


        public List<string> errors { get; set; }

        public TripAdvisorResponse()
        {
            errors = new List<string>();
        }
    }

    [Serializable]
    public class TripAdvisorResponseHotel
    {
        public int hotel_id { get; set; }
        public Dictionary<string, TripAdvisorResponseRoomTypes> room_types { get; set; }
    }

    [Serializable]
    public class TripAdvisorResponseRoomTypes
    {
        public string url { get; set; }
        public decimal price { get; set; }
        public decimal taxes { get; set; }
        public decimal taxes_at_checkout { get; set; }
        public decimal fees { get; set; }
        public decimal fees_at_checkout { get; set; }
        public decimal final_price { get; set; }
        public string currency { get; set; }
        public int num_rooms { get; set; }
    }

    // ReSharper restore InconsistentNaming

}