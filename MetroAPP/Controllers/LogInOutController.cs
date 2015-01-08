using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using MetroAPP.DatabaseAccess;
using MetroAPP.Models.Korisnik;
using MetroAPP.Models.Picture;

namespace MetroAPP.Controllers
{
    public class LogInOutController : Controller
    {
        //
        // GET: /LogInOut/

        public ActionResult Index(Boolean logOut = false)
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];
            if (username == null || password == null)
            {
                return View();
            }
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                try
                {
                    con.Open();

                    var command = new SqlCommand("SEARCHKorisnik", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@KorisnikUsername", username);
                    command.Parameters.AddWithValue("@KorisnikPassword", password);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var mLoginKorisnik = new Korisnik(Int32.Parse(reader["KorisnikId"].ToString()),
                            reader["KorisnikIme"].ToString(), reader["KorisnikPrezime"].ToString(),
                            Convert.ToDateTime(reader["KorisnikRegistracija"].ToString()),
                            reader["KorisnikSlika"].ToString());
                        mLoginKorisnik.KorisnikUsername = reader["KorisnikUsername"].ToString();
                        mLoginKorisnik.KorisnikPassword = reader["KorisnikPassword"].ToString();
                        Session["Korisnik"] = reader["KorisnikId"].ToString();
                        Session["User"] = reader["KorisnikUsername"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                }
            }
            if (Session["Korisnik"] != null)
                return RedirectToAction("Index", "Naslovnica");
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Naslovnica");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registred(Picture picture)
        {
            if (Request.Form["ime"] == String.Empty || Request.Form["prezime"] == String.Empty ||
                Request.Form["username"] == String.Empty || Request.Form["password"] == String.Empty)
                return RedirectToAction("Register");
            try
            {
                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command1 = new SqlCommand("CHECKKorisnik", con);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@KorisnikUsername", Request.Form["username"]);
                    var korisnikExists = (int) command1.ExecuteScalar();
                    if (korisnikExists == 0)
                    {
                        var command = new SqlCommand("INSERTKorisnik", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@KorisnikIme", Request.Form["ime"]);
                        command.Parameters.AddWithValue("@KorisnikPrezime", Request.Form["prezime"]);
                        command.Parameters.AddWithValue("@KorisnikRegistracija", DateTime.Now);
                        command.Parameters.AddWithValue("@KorisnikSlika", picture.File.FileName);
                        command.Parameters.AddWithValue("@KorisnikPassword", Request.Form["password"]);
                        command.Parameters.AddWithValue("@KorisnikUsername", Request.Form["username"]);
                        command.ExecuteNonQuery();
                    }


                }
                if (picture.File.ContentLength > 0)
                {
                    var pic = Path.GetFileName(picture.File.FileName);
                    if (pic != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Images/Korisnik"), pic);
                        // file is uploaded               
                        picture.File.SaveAs(path);
                    }
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return View();
        }
    }
}
