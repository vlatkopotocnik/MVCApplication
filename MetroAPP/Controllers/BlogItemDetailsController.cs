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
    public class BlogItemDetailsController : Controller
    {
        //
        // GET: /BlogItemDetails/

        public ActionResult Index(int blogId)
        {
            var biaaaTop3CommentsAndAllComments = new BlogItemAndKorisnikAndComments();
            var listBiacaa = new List<BlogItemAndKorisnik>();
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                try
                {
                    con.Open();
                    var command1 = new SqlCommand("SELECTAllBlogDetails", con);
                    command1.CommandType = CommandType.StoredProcedure;
                    command1.Parameters.AddWithValue("@BlogId", blogId);
                    var reader1 = command1.ExecuteReader();
                    int mBlogComments = 0;
                    while (reader1.Read())
                    {
                        var biacaa = new BlogItemAndKorisnik();
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

                    foreach (var blogItemAndKorisnik in listBiacaa)
                    {
                        int mKorisnikId = blogItemAndKorisnik.BlogItem.KorisnikId;
                        mBlogComments = blogItemAndKorisnik.BlogItem.BlogId;
                        var command2 = new SqlCommand("SELECTAllBlogKorisnik", con);
                        command2.CommandType = CommandType.StoredProcedure;
                        command2.Parameters.AddWithValue("@KorisnikId", mKorisnikId);
                        var reader2 = command2.ExecuteReader();
                        while (reader2.Read())
                        {
                            var mBlogKorisnik = new Korisnik(Int32.Parse(reader2["KorisnikId"].ToString()),
                                 reader2["KorisnikIme"].ToString(), reader2["KorisnikPrezime"].ToString(),
                                 Convert.ToDateTime(reader2["KorisnikRegistracija"].ToString()), reader2["KorisnikSlika"].ToString());
                            blogItemAndKorisnik.Korisnik = mBlogKorisnik;
                        }
                        reader2.Close();
                    }
                    


                    var command3 = new SqlCommand("SELECTAllBlogRecentComments", con);
                    command3.CommandType = CommandType.StoredProcedure;
                    var reader3 = command3.ExecuteReader();
                    var listTop3CommentAndKorisnik = new List<BlogCommentAndKorisnik>();
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
                        listTop3CommentAndKorisnik.Add(mblogCommentAndKorisnik);
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
                    var command5 = new SqlCommand("SELECTAllBlogComment", con);
                    command5.CommandType = CommandType.StoredProcedure;
                    command5.Parameters.AddWithValue("@CommentIdBlog", mBlogComments);
                    var reader5 = command5.ExecuteReader();
                    var listAllCommentAndKorisnik = new List<BlogCommentAndKorisnik>();
                    var mNumberOfComments = 0;
                    if (reader5.HasRows)
                    {
                        while (reader5.Read())
                        {
                            var mblogComment = new BlogComment(Int32.Parse(reader5["CommentId"].ToString()),
                                Int32.Parse(reader5["CommentIdBlog"].ToString()), reader5["CommentText"].ToString(),
                                Convert.ToDateTime(reader5["CommentDate"]),
                                Int32.Parse(reader5["KorisnikId"].ToString()));
                            var mKorisnik = new Korisnik(Int32.Parse(reader5["KorisnikId"].ToString()),
                                reader5["KorisnikIme"].ToString(), reader5["KorisnikPrezime"].ToString(),
                                Convert.ToDateTime(reader5["KorisnikRegistracija"]), reader5["KorisnikSlika"].ToString());
                            var mblogCommentAndKorisnik = new BlogCommentAndKorisnik();
                            mblogCommentAndKorisnik.BlogComment = mblogComment;
                            mblogCommentAndKorisnik.Korisnik = mKorisnik;
                            listAllCommentAndKorisnik.Add(mblogCommentAndKorisnik);
                            mNumberOfComments++;
                        }
                        reader5.Close();
                    }
                    else
                    {
                        var mblogNoComment = new BlogComment(0, 0, "No comments yet", DateTime.Now, 0);
                        var mblogNoKorisnik = new Korisnik(0, "", "", DateTime.Now, "");
                        var mblogCommentAndKorisnik = new BlogCommentAndKorisnik();
                        mblogCommentAndKorisnik.BlogComment = mblogNoComment;
                        mblogCommentAndKorisnik.Korisnik = mblogNoKorisnik;
                        listAllCommentAndKorisnik.Add(mblogCommentAndKorisnik);
                    }
                    biaaaTop3CommentsAndAllComments.BlogNumberOfComments = mNumberOfComments;
                    biaaaTop3CommentsAndAllComments.ListBlogAllComment = listAllCommentAndKorisnik;
                    biaaaTop3CommentsAndAllComments.ListPaging = mlistPaging;
                    biaaaTop3CommentsAndAllComments.ListBlogItemAndKorisnik = listBiacaa;
                    biaaaTop3CommentsAndAllComments.ListBlogTop3Comment = listTop3CommentAndKorisnik;
                }
                catch (SqlException ex)
                {
                    return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                }
            }
            return View(biaaaTop3CommentsAndAllComments);
        }

        public ActionResult MessagePost()
        {
            using (var con = new SqlConnection(DbHelper.ConnString))
            {
                try
                {
                    con.Open();
                    var command5 = new SqlCommand("INSERTBlogComment", con);
                    command5.CommandType = CommandType.StoredProcedure;
                    command5.Parameters.AddWithValue("@CommentIdBlog", Request.Form["CommentIdBlog"]);
                    command5.Parameters.AddWithValue("@CommentText", Request.Form["message"]);
                    command5.Parameters.AddWithValue("@CommentDate", DateTime.Now);
                    command5.Parameters.AddWithValue("@KorisnikId", Session["Korisnik"]);
                    command5.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    return Redirect(Url.Action("PostError", "Error", new { error = ex.Message }));
                }
            }
            return View("messagePost", null, Request.Form["CommentIdBlog"]);
        }

    }
}
