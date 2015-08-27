using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace Aggregator
{
    /// <summary>
    /// Summary description for MarkUpService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MarkUpService : System.Web.Services.WebService
    {

        [WebMethod]
        public int UpdateMarkUp(int HotelID, int SuperID)
        {
            int RecordUpdate=0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd=new SqlCommand("UpdateMarkUp",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("HotelID", SqlDbType.Int, 50).Value = HotelID;
                    cmd.Parameters.Add("SuperID", SqlDbType.Int, 50).Value = SuperID;
                    if(con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    RecordUpdate = cmd.ExecuteNonQuery();
                }
            }
            return RecordUpdate;
        }

        [WebMethod]
        public DataSet GetDetailByID(int HotelID)
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetDetailByID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("HotelID", SqlDbType.Int, 50).Value = HotelID;
                    
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    adp.Fill(ds);

                    if (con.State== ConnectionState.Open)
                    {
                        con.Close();
                    }
                    
                }
            }
            return ds;
        }

        [WebMethod]
        public int InsertMarkUp(int HotelID, int SuperID)
        {
            int retRecord = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertMarkUp", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("HotelID", SqlDbType.Int, 50).Value = HotelID;
                    cmd.Parameters.Add("SuperID", SqlDbType.Int, 50).Value = SuperID;
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    retRecord = cmd.ExecuteNonQuery();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return retRecord;
        }

        [WebMethod]
        public int DeleteRecord(int HotelID)
        {
            int ReDelete = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteRecordByID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("HotelID", SqlDbType.Int, 50).Value = HotelID;
                    
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    ReDelete = cmd.ExecuteNonQuery();
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return ReDelete;
        }
    }
}
