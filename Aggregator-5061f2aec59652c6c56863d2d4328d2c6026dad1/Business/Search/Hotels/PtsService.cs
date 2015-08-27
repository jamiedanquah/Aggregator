using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Web;
using Aggregator.Objects.Configuration;
using Aggregator.Objects.Search.Hotels;
using Aggregator.PtsSearchService;

namespace Aggregator.Business.Search.Hotels
{
    public static class PtsService
    {
        public static SuperAvailabilityResponse Search(AccommodationSearchRequest request, Settings settings, AggregatorSetting aggregatorSetting, out int searchTime)
        {
            var availabilityRequest = GetRequest(request, aggregatorSetting, settings);

            var authSoapHd = new AuthSoapHd
            {
                strUserName = aggregatorSetting.PtsUserName,
                strNetwork = aggregatorSetting.PtsNetwork,
                strPassword = aggregatorSetting.PtsPassword,
                strUserRef = "",
                strCustomerIP = "" //HttpContext.Current == null ? String.Empty : (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim()
            };


            //"http://172.16.200.174/Tritonroomswsdev/AccommodationServicev2.svc";
            var service = new  AccommodationService { Url = aggregatorSetting.PtsUrl, AuthSoapHdValue = authSoapHd, Timeout = aggregatorSetting.PtsTimeOut };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SuperAvailabilityResponse response =  service.GetSuperAvailability(availabilityRequest);
            stopwatch.Stop();
            searchTime = (int)stopwatch.ElapsedMilliseconds;

            return response;
        }


        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="aggregatorSetting"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static AvailabilityRequest GetRequest(AccommodationSearchRequest request, AggregatorSetting aggregatorSetting, Settings settings)
        {

            var availabilityRequest = new AvailabilityRequest
            {
                HotelId = request.SuperIds == null ? null :  string.Join(",",  request.SuperIds),
                H2H = GetH2H(request.CityCode, aggregatorSetting, settings),
                FeatureCode = aggregatorSetting.PtsFeatureCode,
                DetailLevel = 5,
                CityCode = request.CityCode,
                ConfirmationLevel = 0
            };

            TimeSpan ts = request.CheckOut - request.CheckIn;
            availabilityRequest.Nites = (sbyte)ts.Days;
            availabilityRequest.SvcDate = request.CheckIn;
            availabilityRequest.NoOfAdults = (sbyte)request.NumberOfAdults;
            availabilityRequest.NoOfChildren = (sbyte)(request.ChildAge == null ? 0 : request.ChildAge.Count + request.NumberOfInfants ?? 0);
            if (request.ResortCode.HasValue && request.ResortCode.Value > 0)
                availabilityRequest.Location = (int)request.ResortCode.Value;


            var ages = new List<sbyte>();

            if (request.ChildAge != null)
                foreach (var i in request.ChildAge)
                {
                    ages.Add((sbyte)i);
                }

            for (var i = 0; i < request.NumberOfInfants; i++)
            {
                ages.Add(1);
            }

            availabilityRequest.ChildAge = ages.ToArray();
            availabilityRequest.Units = (sbyte)request.NumberOfRooms;

            return availabilityRequest;
        }

        private static string GetH2H(long cityCode, AggregatorSetting aggregatorSetting, Settings settings)
        {
            if (settings.LookupSettings.H2HToSearch.ContainsKey(cityCode))
                return settings.LookupSettings.H2HToSearch[cityCode];

            return aggregatorSetting.H2HDefault;
        }

        public static TransfersResponseV3 Search(TransfersRequestV3 request, AggregatorSetting aggregatorSetting)
        {

            var authSoapHd = new AuthSoapHd
            {
                strUserName = aggregatorSetting.PtsUserName,
                strNetwork = aggregatorSetting.PtsNetwork,
                strPassword = aggregatorSetting.PtsPassword,
                strUserRef = "",
                strCustomerIP = "" //HttpContext.Current == null ? String.Empty : (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim()
            };

            var service = new AccommodationService { Url = aggregatorSetting.PtsUrl, AuthSoapHdValue = authSoapHd, Timeout = aggregatorSetting.PtsTimeOut };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TransfersResponseV3 response =  service.GetTransfersV3(request);
            stopwatch.Stop();

            return response;
        }
    }
}