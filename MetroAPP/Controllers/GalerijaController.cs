using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using MetroAPP.DatabaseAccess;
using MetroAPP.Models.Galerija;
using MetroAPP.Models.Picture;

namespace MetroAPP.Controllers
{
    public class GalerijaController : Controller
    {
        //
        // GET: /Galerija/

        public ActionResult Index()
        {
            var gliList = new List<GalleryPictureItem>();
            var gliListPages = new HashSet<string>();
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                try
                {
                    con.Open();

                    var command = new SqlCommand("SELECTAllGalleryImage", con);
                    command.CommandType = CommandType.StoredProcedure;
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var a = new GalleryPictureItem(reader["PictureSrc"].ToString(),reader["PicturePage"].ToString(),Int32.Parse(reader["Id"].ToString()));
                        gliList.Add(a);
                        gliListPages.Add(reader["PicturePage"].ToString());
                    }
                    reader.Close();
                }
                catch (SqlException ex)
                {
                    return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                }
            }
            var gpiap = new GalleryPictureItemsAndPage();
            gpiap.GpItems = gliList;
            gpiap.Pages = gliListPages;

            return View(gpiap);
        }

        public ActionResult RemoveItem(int pictureId, string pictureSrc)
        {
            try
            {
                string fullPath = Request.MapPath("~/Images/Gallery/" + pictureSrc);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("DELETEGalleryItem", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PictureId", pictureId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return RedirectToAction("Index", "Galerija");
        }

        public ActionResult AddItem(Picture picture)
        {
            try
            {
                string picturePage = Request.Form["picturePage"];
                var pic = Path.GetFileName(picture.File.FileName);

                var directory = Server.MapPath("~/Images/Gallery/" + Request.Form["picturePage"]);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var path = Path.Combine(Server.MapPath("~/Images/Gallery/" + picturePage), pic);
                // file is uploaded               
                picture.File.SaveAs(path);

                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("INSERTGalleryItem", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PictureSrc",
                        Request.Form["picturePage"] + "/" + picture.File.FileName);
                    command.Parameters.AddWithValue("@PicturePage", Request.Form["picturePage"]);
                    command.Parameters.AddWithValue("@ItemHTMLplace", "Gallery");
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return RedirectToAction("Index", "Galerija");
        }
    }
}
