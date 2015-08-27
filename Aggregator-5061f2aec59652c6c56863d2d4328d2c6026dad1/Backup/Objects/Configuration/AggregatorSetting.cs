namespace Aggregator.Objects.Configuration
{
    public class AggregatorSetting
    {
        public int Id { get; set; }
        public string Tracker { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public int HotelConcurrentSearches { get; set; }
        public string PtsUserName { get; set; }
        public string PtsPassword { get; set; }
        public string PtsNetwork { get; set; }
        public string[] PtsFeatureCode { get; set; }
        public bool LoginSuccess { get; set; }
        public string PtsUrl { get; set; }
        public int PtsTimeOut { get; set; }
        /// <summary>
        /// Message if they failed to login
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// They logged on ok but the account was disabled
        /// </summary>
        public string Warning { get; set; }
        public string H2HDefault { get; set; }

        public int CurrentSearches { get; set;}
    }
}