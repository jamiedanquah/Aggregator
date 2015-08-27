using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Aggregator.Objects.Configuration;
using Aggregator.PtsSearchService;

namespace Aggregator.Business.Search.Hotels
{
    public class TransferService
    {
        public static void GetTransfers(Dictionary<int, decimal> transfers, SuperAvailabilityResponse superAvailabilityResponse, AggregatorSetting aggregatorSetting)
        {
            var threads = new List<ThreadWorking>();
            foreach (var transfer in transfers)
            {
                var transfersRequestV3 = new TransfersRequestV3
                {
                    ClientType = 0,
                    ConfirmationLevel = 0,
                    ChildAge = superAvailabilityResponse.AvailabilityRequest.ChildAge,
                    NoOfAdults = superAvailabilityResponse.AvailabilityRequest.NoOfAdults,
                    NoOfChildren = superAvailabilityResponse.AvailabilityRequest.NoOfChildren,
                    SvcDateFrom = superAvailabilityResponse.AvailabilityRequest.SvcDate,
                    SvcDateTo = superAvailabilityResponse.AvailabilityRequest.SvcDate.AddDays(superAvailabilityResponse.AvailabilityRequest.Nites),
                    LocationID = transfer.Key
                };

                var thread = new ThreadWorking
                {
                    TransferSearch = new TransferSearch(transfersRequestV3, aggregatorSetting)
                };

                thread.Thread = new Thread(thread.TransferSearch.Search);
                thread.Thread.Start();
                threads.Add(thread);
            }

            // join up all the threads
            foreach (var threadWorking in threads)
            {
                threadWorking.Thread.Join();
                transfers[threadWorking.TransferSearch.Request.LocationID] = threadWorking.TransferSearch.CheapestTransferPrice;
            }
        }
    }

    public class ThreadWorking
    {
        internal TransferSearch TransferSearch { get; set; }
        public Thread Thread { get; set; }
    }

    internal class TransferSearch
    {
        public decimal CheapestTransferPrice { get; set; }
        public TransfersRequestV3 Request { get; set; }
        public AggregatorSetting AggregatorSetting { get; set; }

        public TransferSearch(TransfersRequestV3 request, AggregatorSetting aggregatorSetting)
        {
            Request = request;
            AggregatorSetting = aggregatorSetting;
        }


        public void Search()
        {
            // set default
            CheapestTransferPrice = -1;

            var results = PtsService.Search(Request, AggregatorSetting);
            if (results != null && results.TransferV3 != null && results.TransferV3.Any())
            {
                // order the results in price order
                results.TransferV3 = (from data in results.TransferV3
                                      orderby data.BookingPrice
                                      select data).ToArray();


                var cheapestPrice = results.TransferV3[0].BookingPrice;
                // now add the markup on
                //if (AggregatorSetting.TransferMarkupIsPercentage)
                //    cheapestPrice = cheapestPrice * ((AggregatorSetting.TransferMarkup / 100) + 1);
                //else
                //    cheapestPrice = cheapestPrice + AggregatorSetting.TransferMarkup;

                // set the price to be the cheapest one
                CheapestTransferPrice = cheapestPrice;


            }
        }
    }
}