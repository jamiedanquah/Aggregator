using System.Collections.Generic;
using System.Configuration;
using Aggregator.Business.Logging;
using Aggregator.Data;
using Aggregator.Objects.Configuration;

namespace Aggregator.Business.Startup
{
    public class InitializeService
    {
        public static Settings Settings;

        /// <summary>
        /// Used to for workings out as want the settings variable to swap as quick as possible
        /// </summary>
        private static Settings _settings;

        public static void Initialize()
        {
            if (Settings != null)
                return;

            _settings = new Settings();

            // run this first as it has the information in it we need
            GetDatabaseSettings();

            // now get email setting as database hits will send email out if does not work
            EmailSettings();

            // Aggregator settings
            GetAggregatorSettings();

            // get logging settings
            GetLoggingSettings();

            // Get the data used for mark-up etc
            GetLookupSettings();

            _settings.WriteData = new WriteData();

            // set the logging going
            _settings.LoggingSettings.AccommodationJobHost = new AccommodationJobHost(_settings);

            AccommodationTimer.Start(_settings);
        
            // set this last as we need
            Settings = _settings;
        }

        /// <summary>
        /// Gets the database <c>settings</c> that are needed
        /// </summary>
        private static void GetDatabaseSettings()
        {
            _settings.DatabaseSettings = new DatabaseSettings
            {
                ReadConnectionString = ConfigurationManager.ConnectionStrings["ReadData"].ToString(),
                WriteConnectionString = ConfigurationManager.ConnectionStrings["WriteData"].ToString()
            };
        }

        public static void EmailSettings()
        {
            _settings.MailSettings = new MailSettings
                {
                    ErrorMailFrom = ConfigurationManager.AppSettings["ErrorMailFrom"],
                    ErrorMailTo = ConfigurationManager.AppSettings["ErrorMailTo"],
                    SmtpServer = ConfigurationManager.AppSettings["SmtpServer"],
                }; 
        }

        /// <summary>
        /// Get the <c>settings</c> for each aggregator
        /// </summary>
        private static void GetAggregatorSettings()
        {
            _settings.AggregatorSettings = new Dictionary<string, AggregatorSetting>();
            var data = LookupData.GetAggregatorAuthentication(_settings);

            foreach (var aggregatorSetting in data)
            {
                _settings.AggregatorSettings.Add(aggregatorSetting.UserName, aggregatorSetting);
            }
        }

        /// <summary>
        /// Gets the logging <c>settings</c> and sets up the logging for the timer
        /// </summary>
        private static void GetLoggingSettings()
        {
            _settings.LoggingSettings = new LoggingSettings
            {
                NumberOfRowsBeforeLogging = int.Parse(ConfigurationManager.AppSettings["NumberOfRowsBeforeLogging"]),
                TimeInMillisecondsToLog = int.Parse(ConfigurationManager.AppSettings["TimeInMillisecondsToLog"]),
            };
        }

        /// <summary>
        /// Gets the lookup <c>settings</c>
        /// </summary>
        private static void GetLookupSettings()
        {
            _settings.LookupSettings = new LookupSettings
            {
                DestinationMarkup = LookupData.GetDestinationMarkup(_settings),
                HotelMarkup = LookupData.GetHotelMarkup(_settings),
                H2HToSearch = LookupData.GetH2H(_settings),
                CityAsIata = LookupData.CityAsIata(_settings),
            };
        }
    }
}