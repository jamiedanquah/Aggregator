using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Aggregator.Business.Helpers;
using Aggregator.Business.Search.Hotels;
using Aggregator.Business.Startup;
using Aggregator.Objects;
using Aggregator.Objects.Search.Hotels;
using Aggregator.Objects.Search.Hotels.TripAdvisor;

namespace Aggregator.TripAdvisor.Hotel_Availability
{
    [ExcludeFromCodeCoverage]
    public partial class Default : System.Web.UI.Page
    {
        protected override void OnPreLoad(EventArgs e)
        {
            var response = new TripAdvisorResponse();
            var request = new TripAdvisorRequest();
            ParseRequest(request, response);

            if (response.errors.Count == 0)
            {
                var aggResponce = SendSearchRequest(request);
                ParseResultSet(request, response, aggResponce);
            }

            var parser = new JavaScriptSerializer();
            var json = parser.Serialize(response);

            Response.Clear();
            Response.ContentType = "text/json";
            Response.Write(json);

            base.OnPreLoad(e);
        }

        /// <summary>
        /// Parses the <c>request</c> and turns into the <c>object</c> we need
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void ParseRequest(TripAdvisorRequest request, TripAdvisorResponse response)
        {
            ParseApiVersion(request, response);
            ParseHotelIds(request, response);
            ParseStartDate(request, response);
            ParseEndDate(request, response);
            ParseAdults(request, response);
            ParseRooms(request, response);
            ParseQueryKey(request, response);
            ParseCurrency(request);
            ParseUserCountry(request);
            ParseDeviceType(request);
        }

        /// <summary>
        /// Work out the api version they are using
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private static void ParseApiVersion(TripAdvisorRequest request, TripAdvisorResponse response)
        {
            if (HttpContext.Current.Request.Form["api_version"] == null)
            {
                response.errors.Add("api_version required");
                return;
            }

            int testInt;
            if (int.TryParse(HttpContext.Current.Request.Form["api_version"], out testInt))
            {
                request.ApiVersion = testInt;
            }
            else
                response.errors.Add("api_version must be of type int");
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void ParseHotelIds(TripAdvisorRequest request, TripAdvisorResponse response)
        {
            if (HttpContext.Current.Request.Form["hotels"] == null)
            {
                response.errors.Add("hotel_ids required");
                return;
            }

            var parser = new JavaScriptSerializer();
            var dataToParse = HttpContext.Current.Request.Form["hotels"];

            // ReSharper disable AssignNullToNotNullAttribute
            var hotels = parser.Deserialize<List<TripAdvisorRequestHotels>>(Server.UrlDecode(dataToParse));
            // ReSharper restore AssignNullToNotNullAttribute
            request.Hotels = hotels;

        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private static void ParseStartDate(TripAdvisorRequest request, TripAdvisorResponse response)
        {
            if (HttpContext.Current.Request.Form["start_date"] == null)
            {
                response.errors.Add("start_date required");
                return;
            }

            DateTime testDate;
            if (DateTime.TryParse(HttpContext.Current.Request.Form["start_date"], out testDate))
            {
                request.StartDate = testDate;
            }
            else
                response.errors.Add("start_date must be of type date");
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private static void ParseEndDate(TripAdvisorRequest request, TripAdvisorResponse response)
        {
            if (HttpContext.Current.Request.Form["end_date"] == null)
            {
                response.errors.Add("end_date required");
                return;
            }

            DateTime testDate;
            if (DateTime.TryParse(HttpContext.Current.Request.Form["end_date"], out testDate))
            {
                request.EndDate = testDate;
            }
            else
                response.errors.Add("end_date must be of type date");
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private static void ParseAdults(TripAdvisorRequest request, TripAdvisorResponse response)
        {
            if (HttpContext.Current.Request.Form["num_adults"] == null)
            {
                response.errors.Add("num_adults required");
                return;
            }

            int testInt;
            if (int.TryParse(HttpContext.Current.Request.Form["num_adults"], out testInt))
            {
                request.Adults = testInt;
            }
            else
                response.errors.Add("num_adults must be of type int");
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private static void ParseRooms(TripAdvisorRequest request, TripAdvisorResponse response)
        {
            if (HttpContext.Current.Request.Form["num_rooms"] == null)
            {
                response.errors.Add("num_rooms required");
                return;
            }

            int testInt;
            if (int.TryParse(HttpContext.Current.Request.Form["num_rooms"], out testInt))
            {
                request.Rooms = testInt;
            }
            else
                response.errors.Add("num_rooms must be of type int");
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private static void ParseQueryKey(TripAdvisorRequest request, TripAdvisorResponse response)
        {
            if (HttpContext.Current.Request.Form["query_key"] == null)
            {
                response.errors.Add("query_key required");
                return;
            }

            request.QueryKey = HttpContext.Current.Request.Form["query_key"];

        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        private static void ParseCurrency(TripAdvisorRequest request)
        {
            if (HttpContext.Current.Request.Form["currency"] == null)
            {
                return;
            }

            request.currency = HttpContext.Current.Request.Form["currency"];

        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        private static void ParseUserCountry(TripAdvisorRequest request)
        {
            if (HttpContext.Current.Request.Form["user_country"] == null)
            {
                return;
            }
            request.user_country = HttpContext.Current.Request.Form["user_country"];
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        private static void ParseDeviceType(TripAdvisorRequest request)
        {
            if (HttpContext.Current.Request.Form["device_type"] == null)
            {
                return;
            }
            request.device_type = HttpContext.Current.Request.Form["device_type"];
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static AccommodationSearchResponse SendSearchRequest(TripAdvisorRequest request)
        {
            var myUri = new Uri(request.Hotels[0].partner_url);

            var superIds = new List<string>();
            foreach (var hotel in request.Hotels)
                superIds.Add(hotel.partner_id);

            var aggResponce = HotelSearch.Search(new AccommodationSearchRequest
                {
                    Authentication = new Authentication
                        {
                            UserName = "TripAdvisor",
                            Password = "myXyHRT0E67VLmEihxVkfpW2UyB29yM7"
                        },
                    CheckIn = request.StartDate,
                    CheckOut = request.EndDate,
                    ChildAge = new List<int>(),
                    CityCode = long.Parse(HttpUtility.ParseQueryString(myUri.Query).Get("city")),
                    CountryCode = long.Parse(HttpUtility.ParseQueryString(myUri.Query).Get("country")),
                    MinStarRating = null,
                    NumberOfAdults = request.Adults,
                    NumberOfInfants = null,
                    NumberOfRooms = request.Rooms,
                    ResortCode = null,
                    SuperIds = superIds

                }, InitializeService.Settings);

            return aggResponce;
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="accommodation"></param>
        private static void ParseResultSet(TripAdvisorRequest request, TripAdvisorResponse response, AccommodationSearchResponse accommodation)
        {
            try
            {


                // pass any errors over
                response.errors = accommodation.Message.Errors;

                response.api_version = 4;

                response.hotel_ids = new List<int>();
                foreach (var hotels in request.Hotels)
                {
                    response.hotel_ids.Add(hotels.ta_id);
                }

                response.start_date = request.StartDate.ToString("yyyy-MM-dd");
                response.end_date = request.EndDate.ToString("yyyy-MM-dd");
                response.num_adults = request.Adults;
                response.num_rooms = request.Rooms;
                response.currency = request.currency;
                response.user_country = request.user_country;
                response.device_type = request.device_type;
                response.query_key = request.QueryKey;
                response.lang = "en_GB";
                response.hotels = new List<TripAdvisorResponseHotel>();
                response.num_hotels = 0;

                if (accommodation.Results == null)
                    return;


                response.num_hotels = accommodation.Results.Count;

                // hotels here
                foreach (var hotel in accommodation.Results)
                {
                    var returnHotel = new TripAdvisorResponseHotel();

                    foreach (var tripAdvisorRequestHotelse in request.Hotels)
                    {
                        if (tripAdvisorRequestHotelse.partner_id == hotel.SuperId)
                        {
                            returnHotel.hotel_id = tripAdvisorRequestHotelse.ta_id;
                            break;
                        }
                    }

                    returnHotel.room_types = new Dictionary<string, TripAdvisorResponseRoomTypes>();

                    hotel.Rooms = (from rooms in hotel.Rooms
                                   orderby rooms.Price
                                   select rooms).ToList();
                    foreach (var room in hotel.Rooms)
                    {
                        var returnRoom = new TripAdvisorResponseRoomTypes
                            {
                                url = room.DeepLink,
                                price = room.Price,
                                taxes = 0,
                                taxes_at_checkout = 0,
                                fees = 0,
                                fees_at_checkout = 0,
                                final_price = room.Price,
                                currency = "GBP",
                                num_rooms = request.Rooms
                            };

                        if (!returnHotel.room_types.ContainsKey(room.RoomTitle))
                            returnHotel.room_types.Add(room.RoomTitle, returnRoom);
                    }
                    response.hotels.Add(returnHotel);
                }
            }
            catch (Exception exception)
            {
                ErrorHelper.SendErrorEmail(exception, "Error parsing results into trip advisor object " + Environment.NewLine + request, InitializeService.Settings);
            }
        }
    }
}
