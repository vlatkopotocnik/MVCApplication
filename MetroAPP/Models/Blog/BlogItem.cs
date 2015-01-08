using System;

namespace MetroAPP.Models.Blog
{
    public class BlogItem
    {
        public int BlogId;
        public string BlogImageUrl;
        public string BlogNaslovnica;
        public string BlogTextSample;
        public string BlogText;
        public string BlogActionLink;
        public int BlogLikes;
        public int CommentId;
        public int KorisnikId;
        public int BlockPage;
        public DateTime BlogItemDate;

        public BlogItem(int blogId, string blogImageUrl, string blogNaslovnica, string blogTextSample, string blogText, string blogActionLink, int blogLikes, int commentId, int korisnikId, int blockPage, DateTime blogItemDate)
        {
            BlogId = blogId;
            BlogImageUrl = blogImageUrl;
            BlogNaslovnica = blogNaslovnica;
            BlogTextSample = blogTextSample;
            BlogText = blogText;
            BlogActionLink = blogActionLink;
            BlogLikes = blogLikes;
            CommentId = commentId;
            KorisnikId = korisnikId;
            BlockPage = blockPage;
            BlogItemDate = blogItemDate;
        }
    }
}