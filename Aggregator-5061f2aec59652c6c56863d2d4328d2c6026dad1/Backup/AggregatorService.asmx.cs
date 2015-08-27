using System.Web.Services;
using Aggregator.Business.Search.Hotels;
using Aggregator.Business.Startup;
using Aggregator.Objects.Search.Hotels;

namespace Aggregator
{
    /// <summary>
    /// Summary description for AggregatorService
    /// </summary>
    [WebService(Namespace = "http://services.holidaygems.co.uk/AggregatorService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AggregatorService : WebService
    {
        /// <summary>
        /// Allows Searching for accommodation
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [WebMethod]
        public AccommodationSearchResponse AccommodationSearch(AccommodationSearchRequest request)
        {
            return HotelSearch.Search(request, InitializeService.Settings);
        }
    }
}
