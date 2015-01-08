using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MetroAPP.DatabaseAccess;
using MetroAPP.Models.Error;

namespace MetroAPP.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /error/
        public ActionResult Index()
        {
            var errorList = new List<Error>();
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                con.Open();
                var command = new SqlCommand("SELECTAllError", con);
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var error = new Error(Int32.Parse(reader["Id"].ToString()), reader["Message"].ToString(),
                        reader["User"].ToString(),Convert.ToDateTime(reader["Date"].ToString()));
                    errorList.Add(error);
                }
                reader.Close();
            }
            return View(errorList);
        }

        public ActionResult PostError(string error)
        {
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                con.Open();
                var command = new SqlCommand("INSERTError", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Message", error);
                command.Parameters.AddWithValue("@User", Session["User"] == null ? "0" : Session["User"].ToString());
                command.Parameters.AddWithValue("@Date", DateTime.Now);
                command.ExecuteNonQuery();
            }
            return View();
        }

        public ActionResult RemoveError(int errorId)
        {
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                con.Open();
                var command = new SqlCommand("DELETEError", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", errorId);
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index", "Error");
        }

    }
}
