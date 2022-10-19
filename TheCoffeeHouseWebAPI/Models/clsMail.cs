using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace TheCoffeeHouseWebAPI.Models
{
    public class clsMail
    {
        private static readonly string _from = "dellunarhotel@gmail.com";
        private static readonly string _pass = "dellunar123";

        public string Send(string sendto, string subject, string content)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_from);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_from, _pass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}