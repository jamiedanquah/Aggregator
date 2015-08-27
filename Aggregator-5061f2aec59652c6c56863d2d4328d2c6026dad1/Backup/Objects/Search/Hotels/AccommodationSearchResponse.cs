using System;
using System.Collections.Generic;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;

namespace Aggregator.Objects.Search.Hotels
{
    [Serializable]
    public class AccommodationSearchResponse
    {
        public Message Message { get; set; }
        public List<AccommodationSearchResponseHotel> Results { get; set; }
        public Guid SearchResultSetGuid { get; set; }

        public AccommodationSearchResponse()
        {
            Message = new Message { Errors = new List<string>(), Warnings = new List<string>() };
            Results = new List<AccommodationSearchResponseHotel>();
        }
    }

    [Serializable]
    public class AccommodationSearchResponseHotel
    {
        public int HotelResultId { get; set; }
        public string SuperId { get; set; }
        public string HotelName { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Resort { get; set; }
        public List<AccommodationSearchResponseHotelRoom> Rooms { get; set; }

    }

    [Serializable]
    public class AccommodationSearchResponseHotelRoom
    {
        public string BoardType { get; set; }
        public string RoomTitle { get; set; }
        public decimal Price { get; set; }
        public string HotelIdDetails { get; set; }

        /// <summary>
        /// </summary>
        private string deepLink;

        [XmlIgnore]
        public string DeepLink
        {
            set
            {
                deepLink = value;
                var doc = new XmlDocument();
                DeepLinkCData = doc.CreateCDataSection(value);
            }
            get { return deepLink; }
        }

        [XmlElement("DeepLink")]
        public XmlCDataSection DeepLinkCData { get; set; }

    }
}