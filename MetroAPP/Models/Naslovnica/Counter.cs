using System;
using System.Data;
using System.Data.SqlClient;
using MetroAPP.DatabaseAccess;

namespace MetroAPP.Models.Naslovnica
{
    public class Counter
    {
        public int Count;

        public void GetCount()
        {
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                con.Open();
                var command = new SqlCommand("SELECTCount", con);
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while(reader.Read())
                    Count = Int32.Parse(reader["count"].ToString());
                reader.Close();
            }
        }

        public void SetCount()
        {
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                con.Open();
                var command = new SqlCommand("INSERTCount", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Count", Count);
                command.ExecuteNonQuery();
            }
        }
    }
}