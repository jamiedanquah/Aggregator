using System;
using System.Collections.Generic;
using Aggregator.Business.Logging;
using Aggregator.Data;

namespace Aggregator.Objects.Configuration
{
    public class Settings
    {
        public Dictionary<string, AggregatorSetting> AggregatorSettings { get; set; }
        public MailSettings MailSettings { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }
        public LookupSettings LookupSettings { get; set; }
        public LoggingSettings LoggingSettings { get; set; }
        public IWriteData WriteData { get; set; }
    }

    public class MailSettings
    {
        public string SmtpServer { get; set; }
        public string ErrorMailFrom { get; set; }
        public string ErrorMailTo { get; set; }
    }

    public class DatabaseSettings
    {
        public string ReadConnectionString { get; set; }
        public string WriteConnectionString { get; set; }
    }

    public class LookupSettings
    {
        public Dictionary<long, string> H2HToSearch { get; set; }
        public Dictionary<string, List<MarkupDestination>> DestinationMarkup { get; set; }
        public Dictionary<int, MarkupHotel> HotelMarkup { get; set; }
        public Dictionary<int, string> CityAsIata { get; set; }
    }

    public class LoggingSettings
    {
        public int TimeInMillisecondsToLog { get; set; }
        public int NumberOfRowsBeforeLogging { get; set; }
        public AccommodationJobHost AccommodationJobHost { get; set; }
    }

    public class HotelBlockedDateRange
    {
        public int AggregatorId { get; set; }
        public int CityCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}