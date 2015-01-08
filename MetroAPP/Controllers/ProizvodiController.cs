using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MetroAPP.DatabaseAccess;
using MetroAPP.Models.Picture;
using MetroAPP.Models.Proizvodi;

namespace MetroAPP.Controllers
{
    public class ProizvodiController : Controller
    {
        //
        // GET: /Proizvodi/

        public ActionResult Index(int page = 0)
        {
            var proizvodiAndPageAndCategory = new ProizvodiAndPageAndCategory();
            var listProizvod = new List<Proizvod>();
            var pages = new List<int>();
            var categories = new HashSet<string>();
            if (Session["itemNumber"] == null)
                Session["itemNumber"] = 0;
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                con.Open();
                var command = new SqlCommand("SELECTOrSearchProizvod", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProizvodPage", page);
                command.Parameters.AddWithValue("@ProizvodSearch", Request.Form["search"]);
                try
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var proizvod = new Proizvod(Int32.Parse(reader["ProizvodId"].ToString()),
                            reader["ProizvodImgSrc"].ToString(), reader["ProizvodNaziv"].ToString(),
                            Decimal.Parse(reader["ProizvodCijena"].ToString()),
                            Int32.Parse(reader["ProizvodPage"].ToString()), reader["ProizvodCategory"].ToString(),reader["ProizvodOpis"].ToString());
                        listProizvod.Add(proizvod);
                        categories.Add(reader["ProizvodCategory"].ToString());
                    }
                    reader.Close();
                    command = new SqlCommand("SELECTDistinctAllProizvodPages", con);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        pages.Add(Int32.Parse(reader["ProizvodPage"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                }

            }
            if (listProizvod.Count == 0)
            {
                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    int search;
                    Int32.TryParse(Request.Form["search"], out search);
                    var command = new SqlCommand("SELECTOrSearchProizvodId", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProizvodSearch", search);
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var proizvod = new Proizvod(Int32.Parse(reader["ProizvodId"].ToString()),
                                reader["ProizvodImgSrc"].ToString(), reader["ProizvodNaziv"].ToString(),
                                Decimal.Parse(reader["ProizvodCijena"].ToString()),
                                Int32.Parse(reader["ProizvodPage"].ToString()), reader["ProizvodCategory"].ToString(), reader["ProizvodOpis"].ToString());
                            listProizvod.Add(proizvod);
                            categories.Add(reader["ProizvodCategory"].ToString());
                        }
                        reader.Close();
                        if (listProizvod.Count == 0)
                        {
                            var command2 = new SqlCommand("SELECTOrSearchProizvodDescription", con);
                            command2.CommandType = CommandType.StoredProcedure;
                            command2.Parameters.AddWithValue("@ProizvodSearch", Request.Form["search"]);
                            var reader2 = command2.ExecuteReader();
                            while (reader2.Read())
                            {
                                var proizvod = new Proizvod(Int32.Parse(reader2["ProizvodId"].ToString()),
                                    reader2["ProizvodImgSrc"].ToString(), reader2["ProizvodNaziv"].ToString(),
                                    Decimal.Parse(reader2["ProizvodCijena"].ToString()),
                                    Int32.Parse(reader2["ProizvodPage"].ToString()), reader2["ProizvodCategory"].ToString(), reader2["ProizvodOpis"].ToString());
                                listProizvod.Add(proizvod);
                                categories.Add(reader2["ProizvodCategory"].ToString());
                            }
                            reader2.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                    }
                }
            }
            proizvodiAndPageAndCategory.ListProizvodi = listProizvod;
            proizvodiAndPageAndCategory.Pages = pages;
            proizvodiAndPageAndCategory.Category = categories;
            return View(proizvodiAndPageAndCategory);
        }

        public ActionResult AddToCart(int proizvodId, string proizvodImgSrc, string proizvodNaziv, decimal proizvodCijena)
        {
            var itemCart = new Cart();
            var listCart = new List<Cart>(); 
            if(Session["listCart"] != null) 
                listCart = (List<Cart>)Session["listCart"];
            itemCart.ProizvodId = proizvodId;
            itemCart.ProizvodImgSrc = proizvodImgSrc;
            itemCart.ProizvodCijena = proizvodCijena;
            itemCart.ProizvodNaziv = proizvodNaziv;
            listCart.Add(itemCart);
            Session["listCart"] = listCart;
            Session["itemNumber"] = listCart.Count;
            return RedirectToAction("Index", "Proizvodi");
        }

        public ActionResult CartDetails()
        {
            var listCart = (List<Cart>) Session["listCart"];
            var listCartAndTotalPrice = new CartAndTotalPrice(0);
            if (listCart != null)
                foreach (var item in listCart)
                {
                    listCartAndTotalPrice.TotalPrice += item.ProizvodCijena;
                }
            listCartAndTotalPrice.ListCart = listCart;
            return View(listCartAndTotalPrice);
        }

        public ActionResult RemoveItem(int proizvodId = 0)
        {
            var listCart = (List<Cart>)Session["listCart"];
            var listCartAndTotalPrice = new CartAndTotalPrice(0);
            listCart.Remove( listCart.Single(i => i.ProizvodId ==  proizvodId));
            Session["listCart"] = listCart;
            foreach (var item in listCart)
            {
                listCartAndTotalPrice.TotalPrice += item.ProizvodCijena;
            }
            listCartAndTotalPrice.ListCart = listCart;
            Session["itemNumber"] = listCart.Count();
            return RedirectToAction("CartDetails", listCartAndTotalPrice);
        }

        public ActionResult HellsBellsAdd(Picture picture)
        {
            var pic = Path.GetFileName(picture.File.FileName);
            var path = Path.Combine(Server.MapPath("~/Images/Products/" + Request.Form["proizvodKategorija"]), pic);
            try
            {
                // file is uploaded   
                if (!Directory.Exists(Server.MapPath("~/Images/Products/" + Request.Form["proizvodKategorija"])))
                    Directory.CreateDirectory(Server.MapPath("~/Images/Products/" + Request.Form["proizvodKategorija"]));
                picture.File.SaveAs(path);

                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("INSERTHellsBells", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProizvodImgSrc",
                        "/" + Request.Form["proizvodKategorija"] + "/" + picture.File.FileName);
                    command.Parameters.AddWithValue("@ProizvodNaziv", Request.Form["proizvodNaziv"]);
                    command.Parameters.AddWithValue("@ProizvodCijena", Request.Form["proizvodCijena"]);
                    command.Parameters.AddWithValue("@ProizvodPage", Request.Form["proizvodPage"]);
                    command.Parameters.AddWithValue("@ProizvodCategory", Request.Form["proizvodKategorija"]);
                    command.Parameters.AddWithValue("@ProizvodOpis", Request.Form["proizvodOpis"]);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return RedirectToAction("Index", "Proizvodi");
        }

        public ActionResult HellsBellsRemove(int proizvodId, string proizvodImgSrc, string proizvodCategory)
        {
            string fullPath = Request.MapPath("~/Images/Products" + proizvodImgSrc);
            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                if (!Directory.EnumerateFiles(Request.MapPath("~/Images/Products/" + proizvodCategory)).Any())
                    Directory.Delete(Request.MapPath("~/Images/Products/" + proizvodCategory));
                using (var con = new SqlConnection(DbHelper.ConnString))
                {
                    con.Open();
                    var command = new SqlCommand("DELETEHellsBells", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProizvodId", proizvodId);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
            }
            return RedirectToAction("Index", "Proizvodi");
        }
    }
}
