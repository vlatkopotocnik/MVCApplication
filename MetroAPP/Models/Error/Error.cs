using System;

namespace MetroAPP.Models.Error
{
    public class Error
    {
        public int Id;
        public string Message;
        public string User;
        public DateTime Date;

        public Error(int id, string message, string user,DateTime date)
        {
            Id = id;
            Message = message;
            User = user;
            Date = date;
        }
    }
}