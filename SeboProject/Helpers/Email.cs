using System;
using System.Net;
using System.Net.Mail;
namespace SeboProject.Helpers
{
    public static class Email
    {
        private static bool Send(string receiver, string subject, string message)
        {
            try
            {
                var senderEmail = new MailAddress("mycurrencyproject@gmail.com", "My Currency Project");
                var receiverEmail = new MailAddress(receiver, "Dear User");
                var password = "mycurrencypassword";
                var sub = subject;
                var body = message;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}