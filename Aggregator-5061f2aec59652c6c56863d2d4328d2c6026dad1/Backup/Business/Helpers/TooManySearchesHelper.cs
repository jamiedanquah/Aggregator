using System;
using Aggregator.Objects.Configuration;

namespace Aggregator.Business.Helpers
{
    public static class TooManySearchesHelper
    {
        /// <summary>
        /// Used for locking so we have the right amount of searches been reported when having many searches
        /// </summary>
        private static readonly Object ThisLock = new Object();

        public static bool CheckIfWeAreOkToSearch(AggregatorSetting aggregatorSetting, Settings settings)
        {
            AddToSearchCount(aggregatorSetting);

            lock (ThisLock)
            {
                if (aggregatorSetting.CurrentSearches > aggregatorSetting.HotelConcurrentSearches)
                {
                    ErrorHelper.SendErrorEmail(new Exception("Speed limit error"), "Too many searches was carried out: <br />Aggregator: " + aggregatorSetting.UserName + "<br />Current Searches: " + GetCurrentSearchCount(aggregatorSetting) + "<br />Maximum Searches: " + aggregatorSetting.HotelConcurrentSearches, settings);
                    return false;
                }
            }

            return true;
        }

        public static void AddToSearchCount(AggregatorSetting aggregatorSetting)
        {
            lock (ThisLock)
            {
                aggregatorSetting.CurrentSearches++;
            }
        }

        public static void MinusToSearchCount(AggregatorSetting aggregatorSetting)
        {
            lock (ThisLock)
            {
                aggregatorSetting.CurrentSearches--;
            }
        }

        public static int GetCurrentSearchCount(AggregatorSetting aggregatorSetting)
        {
            return aggregatorSetting.CurrentSearches;
        }
    }
}