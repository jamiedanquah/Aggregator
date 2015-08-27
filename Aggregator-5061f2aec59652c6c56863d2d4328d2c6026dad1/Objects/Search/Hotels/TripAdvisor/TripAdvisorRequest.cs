using System;
using System.Collections.Generic;

namespace Aggregator.Objects.Search.Hotels.TripAdvisor
{
    // ReSharper disable InconsistentNaming
    [Serializable]
    public class TripAdvisorRequest
    {
        public int ApiVersion { get; set; }
        public List<TripAdvisorRequestHotels> Hotels { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Adults { get; set; }
        public int Rooms { get; set; }
        public string Lang { get; set; }
        public string QueryKey { get; set; }
        public string currency { get; set; }
        public string user_country { get; set; }
        public string device_type { get; set; }

        public override string ToString()
        {
            var returnString = String.Concat("StartDate: ", StartDate.ToString("yyyy-MM-dd"), " EndDate: ", EndDate.ToString("yyyy-MM-dd"), " Adults: ", Adults);
            foreach (var hotel in Hotels)
            {
                returnString += String.Concat(Environment.NewLine, " ta_id: ", hotel.ta_id, " partner_id: ", hotel.partner_id, " partner_url: ", hotel.partner_url);
            }

            return returnString;
        }
    }

    [Serializable]
    public class TripAdvisorRequestHotels
    {
        public int ta_id { get; set; }
        public string partner_id { get; set; }
        public string partner_url { get; set; }
    }
    // ReSharper restore InconsistentNaming
}