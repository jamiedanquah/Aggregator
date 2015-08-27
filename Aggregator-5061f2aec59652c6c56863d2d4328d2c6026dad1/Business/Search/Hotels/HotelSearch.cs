using System;
using System.Diagnostics;
using Aggregator.Business.Helpers;
using Aggregator.Objects;
using Aggregator.Objects.Configuration;
using Aggregator.Objects.Search.Hotels;
using Aggregator.PtsSearchService;

namespace Aggregator.Business.Search.Hotels
{
    public class HotelSearch
    {
        public static AccommodationSearchResponse Search(AccommodationSearchRequest accommodationSearchRequest, Settings settings)
        {
            // this so we know when we got the request
            var dateOfSearch = DateTime.Now;

            // this is so we can time how long we take to process
            var startOfSearch = new Stopwatch();
            startOfSearch.Start();


            // Check login credentials
            var auth = AuthenticationHelper.CheckAuth(settings, accommodationSearchRequest.Authentication);
            if (!auth.LoginSuccess)
            {
                // they failed to login
                var message = new Message {Success = false};

                // add error message if there
                if (!String.IsNullOrEmpty(auth.Error))
                    message.Errors.Add(auth.Error);

                // add warning if not there
                if (!String.IsNullOrEmpty(auth.Warning))
                    message.Warnings.Add(auth.Warning);

                return new AccommodationSearchResponse {Message = message};
            }

            // now we need to check how many searches they have done
            var areWeOkToSearch = TooManySearchesHelper.CheckIfWeAreOkToSearch(auth, settings);
            if (!areWeOkToSearch)
            {
                TooManySearchesHelper.MinusToSearchCount(auth);
                throw new Exception("Speed Limit: Sorry but we have had too many searches at once please try again in a minute");
            }

            try
            {

                //Debug.WriteLine(TooManySearchesHelper.GetCurrentSearchCount(auth));

                // validate Search
                var errors = ValidationHelper.ValidateSearch(accommodationSearchRequest);
                if (errors.Count != 0)
                {
                    // they failed validation
                    var message = new Message {Success = false};
                    // add error messages in
                    foreach (var error in errors)
                        message.Errors.Add(error);
                    TooManySearchesHelper.MinusToSearchCount(auth);
                    return new AccommodationSearchResponse {Message = message};
                }

                var log = new AccommodationSearchLog();

                // do the search
                var isError = false;
                var errorMessage = String.Empty;
                int timeToSearchPts = 0;
                SuperAvailabilityResponse ptsResults = null;
                try
                {
                    ptsResults = PtsService.Search(accommodationSearchRequest, settings, auth, out timeToSearchPts);
                }
                catch (Exception exception)
                {

                    log.ConvertSearch(accommodationSearchRequest, auth);
                    log.StartOfSearch = dateOfSearch;
                    log.TimeToSearchSupplier = timeToSearchPts;
                    log.TimeToSearchTotal = (int) startOfSearch.ElapsedMilliseconds;
                    log.NumberOfHotelResults = 0;
                    log.Error = exception.Message;
                    settings.LoggingSettings.AccommodationJobHost.AddLog(log);
                    isError = true;
                    errorMessage = exception.Message;
                }
                // process and add in mark-ups
                var processdResults = ProcessService.Process(settings, ptsResults, auth, accommodationSearchRequest.PassengerUrl());
                if (isError)
                {
                    processdResults.Message.Success = false;
                    processdResults.Message.Errors.Add(errorMessage);
                }

                startOfSearch.Stop();
                log.ConvertSearch(accommodationSearchRequest, auth);
                log.StartOfSearch = dateOfSearch;
                log.TimeToSearchSupplier = timeToSearchPts;
                log.TimeToSearchTotal = (int) startOfSearch.ElapsedMilliseconds;
                log.NumberOfHotelResults = processdResults.Results == null ? 0 : processdResults.Results.Count;
                settings.LoggingSettings.AccommodationJobHost.AddLog(log);

                TooManySearchesHelper.MinusToSearchCount(auth);
                return processdResults;
            }
            catch (Exception ex)
            {
                ErrorHelper.SendErrorEmail(ex, "Error In Search", settings);
                TooManySearchesHelper.MinusToSearchCount(auth);
                throw;
            }
        }
    }
}