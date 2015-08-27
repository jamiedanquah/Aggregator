using System;
using System.Collections.Generic;
using System.Linq;
using Aggregator.Objects;
using Aggregator.Objects.Configuration;
using Aggregator.Objects.Search.Hotels;
using Aggregator.PtsSearchService;

namespace Aggregator.Business.Search.Hotels
{
    public class ProcessService
    {
        public static AccommodationSearchResponse Process(Settings settings, SuperAvailabilityResponse superAvailabilityResponse, AggregatorSetting aggregatorSetting, string rooms)
        {
            if (superAvailabilityResponse == null)
            {
                return new AccommodationSearchResponse
                {
                    Message = new Message
                    {
                        Success = false,
                        Errors = new List<string> { "There was no Search results returned due to an error" }
                    },
                    Results = null,
                    SearchResultSetGuid = new Guid()
                };
            }
            var returnHotels = new List<AccommodationSearchResponseHotel>();

            var getTheIataFromCity =  (settings.LookupSettings.CityAsIata.ContainsKey((int) superAvailabilityResponse.AvailabilityRequest.CityCode) ?  settings.LookupSettings.CityAsIata[(int) superAvailabilityResponse.AvailabilityRequest.CityCode] : null) ?? "-1";
            var destinationMarkup = (settings.LookupSettings.DestinationMarkup.ContainsKey(getTheIataFromCity) ? settings.LookupSettings.DestinationMarkup[getTheIataFromCity] : null) ?? settings.LookupSettings.DestinationMarkup["-1"];

            var transfers = new Dictionary<int, decimal>();

            // loop through each hotel
            if (superAvailabilityResponse.SuperHotelAvailabilityDetail != null)
            {
                if (aggregatorSetting.UserName.ToLower() == "tripadvisor")
                {
                    // loop through the hotels and see if there are any compulsory
                    foreach (var hotel in superAvailabilityResponse.SuperHotelAvailabilityDetail)
                    {
                        foreach (var room in hotel.Hotels)
                        {
                            if (room.TransferCompulsory && room.HotelLocationID.HasValue && !transfers.ContainsKey(room.HotelLocationID.Value))
                            {
                                transfers.Add(room.HotelLocationID.Value, -1);
                            }
                        }
                    }

                    TransferService.GetTransfers(transfers, superAvailabilityResponse, aggregatorSetting);
                }

                foreach (var hotel in superAvailabilityResponse.SuperHotelAvailabilityDetail)
                {
                    int star;
                    if (!string.IsNullOrEmpty(hotel.SuperHotelCategory) && int.TryParse(hotel.SuperHotelCategory.Substring(0, 1), out star))
                    {
                        // do nothing
                    }
                    else
                        star = 0;

                    // convert the hotel ready
                    var returnHotel = ConvertService.Convert(hotel, star);
                    var hotelId = int.Parse(hotel.SuperHotelID);
                    var hotelMarkups = settings.LookupSettings.HotelMarkup.ContainsKey(hotelId) ? settings.LookupSettings.HotelMarkup[hotelId] : null;



                    // loop through each room
                    foreach (var room in hotel.Hotels)
                    {
                        // see if we need to find out transfers

                        // hotel markup rules
                        if (hotelMarkups != null)
                        {
                            if (hotelMarkups.IsPercentageMarkup)
                                room.Price += (room.Price/100m)*hotelMarkups.Markup;
                            else
                                room.Price += hotelMarkups.Markup;
                        }
                        else
                        {
                            // now apply a destination markup
                            foreach (var markupDestination in destinationMarkup)
                            {
                                if (room.Price > markupDestination.MinPriceTakesEffect)
                                {
                                    if (markupDestination.IsPercentageMarkup)
                                        room.Price += (room.Price / 100m) * markupDestination.Markup;
                                    else
                                        room.Price += markupDestination.Markup;
                                    break;
                                }
                            }
                        }

                        // apply transfers if needed
                        //if (room.HotelLocationID.HasValue && room.TransferCompulsory && transfers.ContainsKey(room.HotelLocationID.Value))
                        //{
                        //    if (transfers[room.HotelLocationID.Value] == -1)
                        //        continue;
                        //    room.MarkupTotals.FixedTotal += transfers[room.HotelLocationID.Value];/}
                        
                        // round the price off
                        room.Price = Math.Round(room.Price, 2);

                        // convert to the object to return
                        returnHotel.Rooms.Add(ConvertService.Convert(room,  aggregatorSetting, hotel.SuperHotelID, rooms));
                    }

                    if (returnHotel.Rooms.Any())
                        returnHotels.Add(returnHotel);
                }
            }
            var returnResult = new AccommodationSearchResponse
            {
                Message = new Message
                {
                    Success = true
                },
                Results = returnHotels,
                SearchResultSetGuid = superAvailabilityResponse.ResultID == null ? new Guid() : new Guid(superAvailabilityResponse.ResultID)
            };

            return returnResult;
        }
    }
}