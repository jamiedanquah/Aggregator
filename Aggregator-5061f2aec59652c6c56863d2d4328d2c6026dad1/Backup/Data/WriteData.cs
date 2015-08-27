using System;
using System.Data;
using System.Data.SqlClient;
using Aggregator.Business.Helpers;
using Aggregator.Objects.Configuration;

namespace Aggregator.Data
{
    public interface IWriteData
    {
        void WriteLog(DataTable dataTable, Settings settings);
    }

    public class WriteData : IWriteData
    {
        public void WriteLog(DataTable dataTable, Settings settings)
        {
            using (var connection = new SqlConnection(settings.DatabaseSettings.WriteConnectionString))
            {
                try
                {
                    connection.Open();
                    using (var s = new SqlBulkCopy(connection))
                    {
                        s.DestinationTableName = dataTable.TableName;
                        foreach (var column in dataTable.Columns)
                            s.ColumnMappings.Add(column.ToString(), column.ToString());
                        s.WriteToServer(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    ErrorHelper.SendErrorEmail(ex, "Error saving log " + dataTable.TableName, settings);
                    throw;
                }
            }
        }
    }
}