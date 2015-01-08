using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using MetroAPP.DatabaseAccess;
using MetroAPP.Models.Blog;
using MetroAPP.Models.Korisnik;

namespace MetroAPP.Controllers
{
    public class BlogController : Controller
    {
        //
        // GET: /Blog/

        public ActionResult Index(int page = 1)
        {
            var biaaa3Comments = new BlogItemAndKorisnikAndComments();
            var listBiacaa = new List<BlogItemAndKorisnik>();
            var biacaa = new BlogItemAndKorisnik();
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                try
                {
                    con.Open();

                    var command1 = new SqlCommand("SELECTAllBlogItem", con);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@BlockPage", page);
                    var reader1 = command1.ExecuteReader();

                    while (reader1.Read())
                    {
                        var mblogItem = new BlogItem(Int32.Parse(reader1["BlogId"].ToString()),
                            reader1["BlogImageUrl"].ToString(), reader1["BlogNaslovnica"].ToString(),
                            reader1["BlogTextSample"].ToString(), reader1["BlogText"].ToString(),
                            reader1["BlogActionLink"].ToString(), Int32.Parse(reader1["BlogLikes"].ToString()),
                            Int32.Parse(reader1["CommentId"].ToString()), Int32.Parse(reader1["KorisnikId"].ToString()),
                            Int32.Parse(reader1["BlogPage"].ToString()),
                            Convert.ToDateTime(reader1["BlogItemDate"].ToString()));                       
                        biacaa.BlogItem = mblogItem;
                        listBiacaa.Add(biacaa);
                    }
                    reader1.Close();
                    foreach (var listItem in listBiacaa)
                    {
                        int mKorisnikId = listItem.BlogItem.KorisnikId;
                        var command2 = new SqlCommand("SELECTAllBlogKorisnik", con);
                        command2.CommandType = CommandType.StoredProcedure;
                        command2.Parameters.AddWithValue("@KorisnikId", mKorisnikId);
                        var reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            var mBlogKorisnik = new Korisnik(Int32.Parse(reader2["KorisnikId"].ToString()),
                                reader2["KorisnikIme"].ToString(), reader2["KorisnikPrezime"].ToString(),
                                Convert.ToDateTime(reader2["KorisnikRegistracija"].ToString()), reader2["KorisnikSlika"].ToString());
                            biacaa.Korisnik = mBlogKorisnik;
                        }
                        reader2.Close();
                    }
                    


                    var command3 = new SqlCommand("SELECTAllBlogRecentComments", con);
                    command3.CommandType = CommandType.StoredProcedure;
                    var reader3 = command3.ExecuteReader();
                    var listCommentAndKorisnik = new List<BlogCommentAndKorisnik>();
                    while (reader3.Read())
                    {
                        var mblogComment = new BlogComment(Int32.Parse(reader3["CommentId"].ToString()),
                            Int32.Parse(reader3["CommentIdBlog"].ToString()), reader3["CommentText"].ToString(),
                            Convert.ToDateTime(reader3["CommentDate"]), Int32.Parse(reader3["KorisnikId"].ToString()));
                        var mKorisnik = new Korisnik(Int32.Parse(reader3["KorisnikId"].ToString()),
                            reader3["KorisnikIme"].ToString(), reader3["KorisnikPrezime"].ToString(),
                            Convert.ToDateTime(reader3["KorisnikRegistracija"]), reader3["KorisnikSlika"].ToString());
                        var mblogCommentAndKorisnik = new BlogCommentAndKorisnik();
                        mblogCommentAndKorisnik.BlogComment = mblogComment;
                        mblogCommentAndKorisnik.Korisnik = mKorisnik;
                        listCommentAndKorisnik.Add(mblogCommentAndKorisnik);
                    }
                    reader3.Close();
                    var command4 = new SqlCommand("SELECTAllBlogPages", con);
                    command4.CommandType = CommandType.StoredProcedure;
                    var reader4 = command4.ExecuteReader();
                    var mlistPaging = new List<int>();
                    while (reader4.Read())
                    {
                        var stranica = Int32.Parse(reader4["BlogPage"].ToString());
                        mlistPaging.Add(stranica);
                    }
                    reader4.Close();
                    biaaa3Comments.ListPaging = mlistPaging;
                    biaaa3Comments.ListBlogItemAndKorisnik = listBiacaa;
                    biaaa3Comments.ListBlogTop3Comment = listCommentAndKorisnik;
                }
                catch (SqlException ex)
                {
                    return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                }
            }
            return View(biaaa3Comments);
        }

    }
}
