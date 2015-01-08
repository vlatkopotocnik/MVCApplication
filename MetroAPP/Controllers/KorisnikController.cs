using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using MetroAPP.DatabaseAccess;
using MetroAPP.Models.Korisnik;
using MetroAPP.Models.Picture;

namespace MetroAPP.Controllers
{
    public class KorisnikController : Controller
    {
        //
        // GET: /Korisnik/

        public ActionResult Index()
        {
            var listKorisnik = new List<Korisnik>();
            try
            {
                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("SELECTAllKorisnik", con);
                    command.CommandType = CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var korisnik = new Korisnik(Int32.Parse(reader["KorisnikId"].ToString()),
                            reader["KorisnikIme"].ToString(), reader["KorisnikPrezime"].ToString(),
                            Convert.ToDateTime(reader["KorisnikRegistracija"].ToString()),
                            reader["KorisnikSlika"].ToString());
                        korisnik.KorisnikUsername = reader["KorisnikUsername"].ToString();
                        korisnik.KorisnikPassword = reader["KorisnikPassword"].ToString();
                        korisnik.KorisnikPasswordSalt = reader["KorisnikPasswordSalt"].ToString();
                        listKorisnik.Add(korisnik);
                    }
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return View(listKorisnik);
        }
        public ActionResult AddItem(Picture picture)
        {
            var pic = Path.GetFileName(picture.File.FileName);
            var path = Path.Combine(Server.MapPath("~/Images/Korisnik/"), pic);
            try
            {
                // file is uploaded               
                picture.File.SaveAs(path);

                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("INSERTKorisnik", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@KorisnikIme", Request.Form["korisnikIme"]);
                    command.Parameters.AddWithValue("@KorisnikPrezime", Request.Form["korisnikPrezime"]);
                    command.Parameters.AddWithValue("@KorisnikRegistracija", DateTime.Now);
                    command.Parameters.AddWithValue("@KorisnikSlika", picture.File.FileName);
                    command.Parameters.AddWithValue("@KorisnikPassword", Request.Form["korisnikPassword"]);
                    command.Parameters.AddWithValue("@KorisnikUsername", Request.Form["korisnikUsername"]);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return RedirectToAction("Index", "Korisnik");
        }
        public ActionResult RemoveItem(int korisnikId, string korisnikSlika)
        {
            string fullPath = Request.MapPath("~/Images/Korisnik/" + korisnikSlika);
            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("DELETEKorisnikItem", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@KorisnikId", korisnikId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return RedirectToAction("Index", "Korisnik");
        }

    }
}
