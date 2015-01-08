using System;

namespace MetroAPP.Models.Blog
{
    public class BlogComment
    {
        public int CommentId;
        public int BlogId;
        public string CommentText;
        public DateTime CommentDate;
        public int KorisnikId;

        public BlogComment(int commentId, int blogId, string commentText, DateTime commentDate, int korinsikId)
        {
            CommentId = commentId;
            CommentText = commentText;
            BlogId = blogId;
            CommentDate = commentDate;
            KorisnikId = korinsikId;
        }
    }
}