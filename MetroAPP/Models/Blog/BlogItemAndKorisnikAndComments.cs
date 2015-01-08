using System.Collections.Generic;

namespace MetroAPP.Models.Blog
{
    public class BlogItemAndKorisnikAndComments
    {
        public List<BlogItemAndKorisnik> ListBlogItemAndKorisnik;
        public List<BlogCommentAndKorisnik> ListBlogTop3Comment;
        public List<BlogCommentAndKorisnik> ListBlogAllComment;
        public int BlogNumberOfComments;
        public List<int> ListPaging;
    }
}