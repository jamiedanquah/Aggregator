using System;
using System.Collections.Generic;
using Aggregator.Objects.Configuration;
using Aggregator.Objects.Search.Hotels;
using Aggregator.PtsSearchService;

namespace Aggregator.Business.Search.Hotels
{
    public class ConvertService
    {
       
    public static AccommodationSearchResponseHotel Convert(SuperAvailabilityResponseSuperHotelAvailabilityDetail hotel, int star)
        {
            var returnValue = new AccommodationSearchResponseHotel
                {
                    City = hotel.SuperCityName,
                    Country = hotel.SuperCountryName,
                    Description = hotel.SuperDescImage.SupDesc,
                    HotelName = hotel.SuperHotelName,
                    HotelResultId = int.Parse(hotel.SuperHotelID),
                    Image = hotel.SuperDescImage.SupImagePath,
                    Resort = hotel.Hotels[0].HotelLocationName,
                    StarRating = star,
                    SuperId = hotel.SuperHotelID,
                    Rooms = new List<AccommodationSearchResponseHotelRoom>()
                };

            return returnValue;
        }

        public static AccommodationSearchResponseHotelRoom Convert(SuperAvailabilityResponseSuperHotelAvailabilityDetailHotels room, AggregatorSetting aggregatorSetting, string superId, string rooms)
        {
            // does not like the &
            // ReSharper disable StringLiteralsWordIsNotInDictionary
             //var url =
             //           String.Concat("http://www.travelbag.co.uk/Waiting/DisplayAccomResults?",
             //           "country=", criteria.AccomCountryCode,
             //           "&city=", criteria.AccomCityCode,

             //           "&location=", room.HotelLocationID,

             //           "&startdate=", criteria.HolidayStartDate.ToString("yyyyMMdd"),
             //           "&enddate=", endDate.ToString("yyyyMMdd"),
             //           "&rooms=", rooms,
             //           "&starrating=-1&boardbasis=-1",
             //           "&ptsid=", superId,
             //           "&pid=", aggregatorSetting.UserName.ToLower());
             // ReSharper restore StringLiteralsWordIsNotInDictionary

            var url = "http://www.holidaygems.co.uk";

            var returnValue = new AccommodationSearchResponseHotelRoom
                {
                    BoardType = room.Room_Board_Type,
                    Price = room.Price,
                    DeepLink = url,
                    RoomTitle = room.Room_Title,
                    HotelIdDetails = room.HotelID
                };
            return returnValue;
        }
    }
}