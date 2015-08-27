using System;
using System.Data;
using System.Threading;
using System.Web.Hosting;
using Aggregator.Objects.Configuration;
using Aggregator.Objects.Search.Hotels;

namespace Aggregator.Business.Logging
{
    public class AccommodationJobHost : IRegisteredObject
    {
        /// <summary>
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// </summary>
        private static DataTable _dt;

        /// <summary>
        /// </summary>
        private readonly Settings settings;

        public AccommodationJobHost(Settings settings)
        {
            this.settings = settings;
            HostingEnvironment.RegisterObject(this);
            SetTableCols();
        }

        /// <summary>
        /// re set the table columns as there can be a case where we are adding
        /// </summary>
        private static void SetTableCols()
        {
            _dt = new DataTable("AccommodationSearch");
            _dt.Columns.Add("Tracker", typeof(string));
            _dt.Columns.Add("NumberOfRooms", typeof(int));
            _dt.Columns.Add("NumberOfAdults", typeof(int));
            _dt.Columns.Add("NumberOfChildren", typeof(int));
            _dt.Columns.Add("NumberOfInfants", typeof(int));
            _dt.Columns.Add("ChildAges", typeof(string));
            _dt.Columns.Add("MinStarRating", typeof(int));
            _dt.Columns.Add("CheckIn", typeof(DateTime));
            _dt.Columns.Add("CheckOut", typeof(DateTime));
            _dt.Columns.Add("CityCode", typeof(long));
            _dt.Columns.Add("ResortCode", typeof(long));
            _dt.Columns.Add("SuperIds", typeof(string));
            _dt.Columns.Add("StartOfSearch", typeof(DateTime));
            _dt.Columns.Add("TimeToSearchSupplier", typeof(int));
            _dt.Columns.Add("TimeToSearchTotal", typeof(int));
            _dt.Columns.Add("NumberOfHotelResults", typeof(int));
            _dt.Columns.Add("Error", typeof(string));
        }

        public void Stop(bool immediate)
        {
            SaveAndClearLog();
            HostingEnvironment.UnregisterObject(this);
        }


        public void AddLog(AccommodationSearchLog log)
        {
            lock (_lock)
            {

                DataRow dr = _dt.NewRow();
                dr.BeginEdit();
                dr["Tracker"] = log.Tracker;
                dr["NumberOfRooms"] = log.NumberOfRooms;
                dr["NumberOfAdults"] = log.NumberOfAdults;
                dr["NumberOfChildren"] = log.NumberOfChildren;
                dr["NumberOfInfants"] = log.NumberOfInfants;
                dr["ChildAges"] = log.ChildAges;
                dr["MinStarRating"] = log.MinStarRating ?? 0;
                dr["CheckIn"] = log.CheckIn;
                dr["CheckOut"] = log.CheckOut;
                dr["CityCode"] = log.CityCode;
                dr["ResortCode"] = log.ResortCode;
                dr["SuperIds"] = log.SuperIds;
                dr["StartOfSearch"] = log.StartOfSearch;
                dr["TimeToSearchSupplier"] = log.TimeToSearchSupplier;
                dr["TimeToSearchTotal"] = log.TimeToSearchTotal;
                dr["NumberOfHotelResults"] = log.NumberOfHotelResults;
                dr["Error"] = log.Error;
                dr.EndEdit();
                _dt.Rows.Add(dr);

                if (_dt.Rows.Count > settings.LoggingSettings.NumberOfRowsBeforeLogging)
                {
                    new Thread(SaveAndClearLog).Start();
                }
            }
        }

        public int GetLogCount()
        {
            lock (_lock)
            {
                return _dt.Rows.Count;
            }
        }

        public void SaveAndClearLog()
        {
            lock (_lock)
            {
                if (_dt.Rows != null && _dt.Rows.Count != 0)
                {
                    settings.WriteData.WriteLog(_dt, settings);
                    _dt.Clear();
                    SetTableCols();
                }
            }
        }
    }

    public static class AccommodationTimer
    {
        /// <summary>
        /// </summary>
        private static AccommodationJobHost _accommodationJobHost;

        public static void Start(Settings settings)
        {
            _accommodationJobHost = settings.LoggingSettings.AccommodationJobHost;
            var timer = new System.Timers.Timer
            {
                Interval = settings.LoggingSettings.TimeInMillisecondsToLog,
                Enabled = true
            };
            timer.Elapsed += timScheduledTask_Elapsed;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _accommodationJobHost.SaveAndClearLog();
        }
    }
}