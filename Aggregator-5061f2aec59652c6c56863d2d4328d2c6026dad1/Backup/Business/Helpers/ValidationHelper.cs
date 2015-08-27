using System;
using System.Collections.Generic;
using Aggregator.Objects.Search.Hotels;

namespace Aggregator.Business.Helpers
{
    public class ValidationHelper
    {
        public static List<string> ValidateSearch(AccommodationSearchRequest accommodationSearchRequest)
        {
            var errors = new List<string>();

            // first check in
            if (accommodationSearchRequest.CheckIn < DateTime.Now.AddDays(3))
                errors.Add("Check-in must be more that 3 days from now");

            // check out
            if (accommodationSearchRequest.CheckIn > accommodationSearchRequest.CheckOut)
                errors.Add("Check-in must be before checkout");

            if (accommodationSearchRequest.CountryCode < 0)
                errors.Add("Please provide valid country code");

            if (accommodationSearchRequest.CityCode < 0)
                errors.Add("Please provide valid city code");

            if (accommodationSearchRequest.MinStarRating.HasValue)
            {
                if (accommodationSearchRequest.MinStarRating > 5)
                    errors.Add("Min star rating can not be greater than 5");

                if (accommodationSearchRequest.MinStarRating < 0)
                    errors.Add("Min star rating can not be less than 0");
            }

            if (accommodationSearchRequest.PassengerCount() > 10)
                errors.Add("The maximum number of passengers per search is 10");

            if (accommodationSearchRequest.NumberOfRooms < 1 || accommodationSearchRequest.NumberOfRooms > 4)
                errors.Add("Number of rooms must be between 1 and 4");

            if (accommodationSearchRequest.NumberOfAdults < 1)
                errors.Add("There must be 1 or more adults travelling");

            if (accommodationSearchRequest.ChildAge != null)
            {
                var i = 0;
                foreach (var age in accommodationSearchRequest.ChildAge)
                {
                    i++;
                    if (age < 2)
                        errors.Add("Child " + i + " must be 2 or older");
                    if (age > 16)
                        errors.Add("Child " + i + " must be 16 or younger");
                }
            }

            if (accommodationSearchRequest.NumberOfRooms > 1 && accommodationSearchRequest.ChildAge != null && accommodationSearchRequest.ChildAge.Count != 0)
                errors.Add("Sorry but we don't currently support more than 1 room with children");

            return errors;
        }
    }
}