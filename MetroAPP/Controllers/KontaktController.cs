using System;
using System.Configuration;
using System.Net.Mail;
using System.Web.Mvc;

namespace MetroAPP.Controllers
{
    public class KontaktController : Controller
    {
        //
        // GET: /Kontakt/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendMessage()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mail = new MailMessage();
                    mail.To.Add(ConfigurationManager.AppSettings["EmailTo"]);
                    mail.From = new MailAddress(Request.Form["email"]);
                    mail.Subject = Request.Form["subject"];
                    string body = String.Concat("Name: ", "<br/>", Request.Form["name"], "<br/>", "<br/>",
                        "Message: ",
                        Request.Form["message"], "<br/>", "<br/>", "Phone: ", Request.Form["phone"], "<br/>",
                        "Company name: ",
                        Request.Form["companyName"]);
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    var smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential
                        (ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]); // Enter seders User name and password
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

    }
}
