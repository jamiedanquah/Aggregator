using System;
using System.Collections.Generic;
using System.Text;

namespace Aggregator.Objects.Search.Hotels
{
    public class AccommodationSearchRequest
    {
        public Authentication Authentication { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfAdults { get; set; }
        public int? NumberOfInfants { get; set; }
        public List<int> ChildAge { get; set; }
        public short? MinStarRating { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public long CountryCode { get; set; }
        public long CityCode { get; set; }
        public long? ResortCode { get; set; }
        public List<string> SuperIds { get; set; }

        /// <summary>
        /// Works out the how many passengers there are based on adults, children and infants
        /// </summary>
        /// <returns>Total number of passengers in the party</returns>
        internal int PassengerCount()
        {
            var count = NumberOfAdults*NumberOfRooms;

            if (NumberOfInfants.HasValue)
                count += NumberOfInfants.Value*NumberOfRooms;

            if (ChildAge != null && ChildAge.Count != 0)
                count += ChildAge.Count;

            return count;
        }

        /// <summary>
        /// Works out what the string should be to work out the passengers
        /// </summary>
        /// <returns></returns>
        internal string PassengerUrl()
        {
            var rooms = new StringBuilder();

            var isFirstFirst = true;

            for (int i = 0; i < NumberOfRooms; i++)
            {
                if (isFirstFirst)
                    isFirstFirst = false;
                else
                    rooms.Append("*");

                rooms.Append(NumberOfAdults);
                if (ChildAge.Count == 0)
                {
                    rooms.Append("-0-");
                }
                else
                {
                    rooms.Append("-");
                    var isFirst = true;
                    foreach (var i1 in ChildAge)
                    {
                        if (isFirst)
                            isFirst = false;
                        else
                            rooms.Append(".");
                        rooms.Append(i1);
                    }
                    rooms.Append("-");
                }

                rooms.Append(NumberOfInfants);
            }

            return rooms.ToString();
        }
    }
}