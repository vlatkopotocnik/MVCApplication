namespace MetroAPP.Models.Kontakt
{
    public class EmailItem
    {
        public string Name;
        public string Email;
        public string Phone;
        public string CompanyName;
        public string Subject;
        public string Message;

        public EmailItem(string name, string email, string phone, string companyName, string subject, string message)
        {
            Name = name;
            Email = email;
            Phone = phone;
            CompanyName = companyName;
            Subject = subject;
            Message = message;
        }

        public EmailItem() { }
    }
}