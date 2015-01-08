using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;
using System.Web.Mvc;
using MetroAPP.DatabaseAccess;
using MetroAPP.Models.Naslovnica;
using MetroAPP.Models.Picture;

namespace MetroAPP.Controllers
{
    public class NaslovnicaController : Controller
    {
        //
        // GET: /Naslovnica/

        public ActionResult Index()
        {
            if (Session["visit"] == null)
            {
                var counter = new Counter();
                counter.GetCount();
                counter.Count++;
                counter.SetCount();
                Session["visit"] = counter.Count;
            }
            
            var naslovnicaDataListItem = new List<NaslovnicaDataListItem>();
            var naslovnicaSliderListItem = new List<NaslovnicaSliderItem>();
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                try
                {
                    con.Open();

                    var command = new SqlCommand("SELECTAllNaslovnicaDataListItem", con);
                    command.CommandType = CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var a = new NaslovnicaDataListItem(Int32.Parse(reader["Id"].ToString()), reader["NaslovnicaItemNaslov"].ToString(), reader["NaslovnicaItemText"].ToString(), reader["PictureSrc"].ToString(), reader["NaslovnicaItemActionLink"].ToString());
                        naslovnicaDataListItem.Add(a);
                    }
                    reader.Close();
                    command = new SqlCommand("SELECTAllNaslovnicaSliderListItem", con);
                    command.CommandType = CommandType.StoredProcedure;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var a = new NaslovnicaSliderItem(Int32.Parse(reader["Id"].ToString()), reader["PictureSrc"].ToString(), reader["NaslovnicaItemNaslov"].ToString(), reader["NaslovnicaItemText"].ToString(), reader["NaslovnicaItemActionLink"].ToString(), reader["SliderListItemActive"].ToString());
                        naslovnicaSliderListItem.Add(a);
                    }
                    reader.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                }
            }
            var ndlasi = new NaslovnicaDataListAndSliderItemList();
            ndlasi.NaslovnicaDataListItems = naslovnicaDataListItem;
            ndlasi.NaslovnicaSliderItems = naslovnicaSliderListItem;
            return View(ndlasi);
        }

        [HttpPost]
        public ActionResult SendMail(string email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mail = new MailMessage();
                    mail.To.Add(ConfigurationManager.AppSettings["EmailTo"]);
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailTo"]);
                    mail.Subject = "Subscriber";
                    string body = email;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    var smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential
                        (ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                }
            }
            return View();
        }
        public ActionResult AddItemNaslovnica(Picture picture)
        {
            var pic = Path.GetFileName(picture.File.FileName);
            var pictureSrc = picture.File.FileName;
            if (Request.Form["directory"] == "Slider")
            {
                pictureSrc = "'/Images/Naslovnica/Slider/" + picture.File.FileName + "'";
            }
            var path = Path.Combine(Server.MapPath("~/Images/Naslovnica/" + Request.Form["directory"] + "/"), pic);
            try
            {
                // file is uploaded               
                picture.File.SaveAs(path);

                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("INSERTNaslovnicaItem", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PictureSrc", pictureSrc);
                    command.Parameters.AddWithValue("@NaslovnicaItemNaslov", Request.Form["pictureTitle"]);
                    command.Parameters.AddWithValue("@NaslovnicaItemText", Request.Form["pictureText"]);
                    command.Parameters.AddWithValue("@NaslovnicaItemActionLink", Request.Form["actionLink"]);
                    command.Parameters.AddWithValue("@SliderListItemActive", Request.Form["itemActive"]);
                    command.Parameters.AddWithValue("@ItemHTMLplace", Request.Form["itemHtmLplace"]);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return RedirectToAction("Index", "Naslovnica");
        }
        public ActionResult RemoveItemNaslovnica(int pictureId, string pictureSrc, string directory)
        {
            var truePictureSrc = pictureSrc;
            if (directory == "Slider")
            {
                truePictureSrc = pictureSrc.Substring(pictureSrc.LastIndexOf('/') + 1);
                truePictureSrc = truePictureSrc.TrimEnd('\'');
            }
            string fullPath = Request.MapPath("~/Images/Naslovnica/" + directory + "/" + truePictureSrc);
            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("DELETENaslovnicaItem", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", pictureId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return RedirectToAction("Index", "Naslovnica");
        }
    }
}
