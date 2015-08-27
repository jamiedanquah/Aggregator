using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Aggregator.Business.Helpers;
using Aggregator.Objects.Configuration;

namespace Aggregator.Data
{
    public class LookupData
    {
        public static List<AggregatorSetting> GetAggregatorAuthentication(Settings settings)
        {
            var returnData = new List<AggregatorSetting>();

            // Create and open the connection in a using block. This 
            // ensures that all resources will be closed and disposed 
            // when the code exits. 
            using (var connection = new SqlConnection(settings.DatabaseSettings.ReadConnectionString))
            {
                // Create the Command and Parameter objects.
                var command = new SqlCommand("dbo.spAuthenticationGetAll", connection) {CommandType = CommandType.StoredProcedure};

                // Open the connection in a try/catch block.  
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var setting = new AggregatorSetting
                        {
                            Enabled = (bool)reader["Enabled"],
                            H2HDefault = (string)reader["H2HDefault"],
                            UserName = (string)reader["UserName"],
                            Password = (string)reader["Password"],
                            Id = (int)reader["Id"],
                            PtsPassword = (string)reader["PTSPassword"],
                            PtsUserName = (string)reader["PTSUserName"],
                            HotelConcurrentSearches = (int)reader["HotelConcurrentSearches"],
                            Tracker = (string)reader["Tracker"],
                            PtsNetwork = (string)reader["PtsNetwork"],
                            PtsTimeOut = (int)reader["PtsTimeOut"],
                            PtsUrl = (string)reader["PtsUrl"],
                        };
                        var featureCode = (string)reader["PtsFeatureCode"];
                        setting.PtsFeatureCode = featureCode.Contains(",") ? featureCode.Split(',') : new[] { featureCode };
                        
                        returnData.Add(setting);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ErrorHelper.SendErrorEmail(ex, "Error getting GetAggregatorAuthentication", settings);
                }

                return returnData;
            }
        }

        internal static Dictionary<string, List<MarkupDestination>> GetDestinationMarkup(Settings settings)
        {
            var returnData = new Dictionary<string, List<MarkupDestination>>();

            // Create and open the connection in a using block. This 
            // ensures that all resources will be closed and disposed 
            // when the code exits. 
            using (var connection = new SqlConnection(settings.DatabaseSettings.ReadConnectionString))
            {
                // Create the Command and Parameter objects.
                var command = new SqlCommand("dbo.[spMarkupDestinationGetAll]", connection) { CommandType = CommandType.StoredProcedure };

                // Open the connection in a try/catch block.  
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var key = (string) reader["Iata"];

                        if (returnData.ContainsKey(key))
                        {
                            returnData[key].Add(new MarkupDestination
                                {
                                    Iata = key,
                                    Id = (int) reader["Id"],
                                    IsPercentageMarkup = (bool) reader["IsPercentageMarkup"],
                                    Markup = (decimal) reader["Markup"],
                                    MinPriceTakesEffect = (decimal) reader["MinPriceTakesEffect"]
                                });
                        }
                        else
                        {
                            returnData.Add(key, new List<MarkupDestination>
                                {
                                    new MarkupDestination
                                        {
                                            Iata = key,
                                            Id = (int) reader["Id"],
                                            IsPercentageMarkup = (bool) reader["IsPercentageMarkup"],
                                            Markup = (decimal) reader["Markup"],
                                            MinPriceTakesEffect = (decimal) reader["MinPriceTakesEffect"]
                                        }
                                });
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ErrorHelper.SendErrorEmail(ex, "Error getting GetDestinationMarkup", settings);
                }

                return returnData;
            }

        }

        internal static Dictionary<int, MarkupHotel> GetHotelMarkup(Settings settings)
        {
            var returnData = new Dictionary<int, MarkupHotel>();

            // Create and open the connection in a using block. This 
            // ensures that all resources will be closed and disposed 
            // when the code exits. 
            using (var connection = new SqlConnection(settings.DatabaseSettings.ReadConnectionString))
            {
                // Create the Command and Parameter objects.
                var command = new SqlCommand("dbo.[spMarkupHotelGetAll]", connection) { CommandType = CommandType.StoredProcedure };

                // Open the connection in a try/catch block.  
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var key = (int)reader["SuperId"];

                        returnData.Add(key, new MarkupHotel
                            {
                                Id = (int)reader["Id"],
                                IsPercentageMarkup = (bool)reader["IsPercentageMarkup"],
                                Markup = (decimal)reader["Markup"],
                                SuperId = key
                            });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ErrorHelper.SendErrorEmail(ex, "Error getting GetHotelMarkup", settings);
                }

                return returnData;
            }
        }

        internal static Dictionary<long, string> GetH2H(Settings settings)
        {
            var returnData = new Dictionary<long, string>();

            // Create and open the connection in a using block. This 
            // ensures that all resources will be closed and disposed 
            // when the code exits. 
            using (var connection = new SqlConnection(settings.DatabaseSettings.ReadConnectionString))
            {
                // Create the Command and Parameter objects.
                var command = new SqlCommand("dbo.[spH2hGetAll]", connection) { CommandType = CommandType.StoredProcedure };

                // Open the connection in a try/catch block.  
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                       returnData.Add((long)reader["CityCode"],(string)reader["H2h"]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ErrorHelper.SendErrorEmail(ex, "Error getting GetH2H", settings);
                }

                return returnData;
            }
        }

        internal static Dictionary<int, string> CityAsIata(Settings settings)
        {
            var returnData = new Dictionary<int, string>();

            // Create and open the connection in a using block. This 
            // ensures that all resources will be closed and disposed 
            // when the code exits. 
            using (var connection = new SqlConnection(settings.DatabaseSettings.ReadConnectionString))
            {
                // Create the Command and Parameter objects.
                var command = new SqlCommand("dbo.[spPtsCityGetAll]", connection) { CommandType = CommandType.StoredProcedure };

                // Open the connection in a try/catch block.  
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add((int)reader["CityCode"], (string)reader["Iata"]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ErrorHelper.SendErrorEmail(ex, "Error getting CityAsIata", settings);
                }

                return returnData;
            }
        }
    }
}