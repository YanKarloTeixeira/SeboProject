using SeboProject.Models;
using System;
using System.Net;
using System.Net.Mail;
namespace SeboProject.Helpers
{
    public static class SeboEmail
    {
        public static bool Send(string receiver, string subject, string message)
        {
            try
            {
                var senderEmail = new MailAddress("projectsebo@gmail.com", "Sebo Project");
                var receiverEmail = new MailAddress(receiver, "Dear User");
                var password = "@Seboproject123";
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

         
        public static bool SendReceiptToBuyer(Order order, User buyer, User seller, Book book)
        {
            string sub = GetSubject(order);
            string msg = GetBuyerMsg(order, seller, book);
            return Send(buyer.Email, sub, msg);
                
        }

        public static bool SendReceiptToSeller(Order order, User buyer, User seller, Book book)
        {
            string sub = GetSubject(order);
            string msg = GetSellerMsg(order, seller, book);
            return Send(buyer.Email, sub, msg);

        }


        private static string GetSubject(Order order)
        {
            return "SEBO Transaction Receipt - Order : " + order.OrderId;
        }
        private static string GetBuyerMsg(Order order, User seller, Book book)
        {
            string msg = null;
            msg += "********************(This is an authomatic message, please do not reply it.)******************** Environment.NewLineEnvironment.NewLineEnvironment.NewLineEnvironment.NewLine";
            msg += "CONGRATULATIONS ! Your transaction is successfuly done.Environment.NewLineEnvironment.NewLine";
            msg += "These are the seller's info for contacts:";
            msg += "-NAME....:" + seller.UserName + " " + seller.MiddleName + " " + seller.LastName + Environment.NewLine;
            msg += "-EMAIL...:" + seller.Email + Environment.NewLine;
            msg += "-PHONE...:" + seller.Phone + Environment.NewLine;
            msg += Environment.NewLine+Environment.NewLine+Environment.NewLine;
            msg += "Purchase information :" + Environment.NewLine;
            msg += "=>ORDER # .........:" + order.OrderId.ToString() + Environment.NewLine;
            msg += "=>Product ID.......:" + book.BookId.ToString() + Environment.NewLine;
            msg += "=>Title............:" + book.Title + Environment.NewLine;
            msg += "=>Study Area.......:" + book.StudyArea.StudyAreaName + Environment.NewLine;
            msg += "=>Book Condition...:" + book.BookCondition.Condition + Environment.NewLine;
            msg += "=>Quantity.........:" + order.Quantity + Environment.NewLine;
            msg += "=>Price............:" + order.Price + Environment.NewLine;
            msg += "----------------------------------------------" + Environment.NewLine;
            msg += "=>Total............:" + order.Quantity * order.Price + Environment.NewLine;

            return msg;

        }
        private static string GetSellerMsg(Order order, User buyer, Book book)
        {
            string msg = null;
            msg += "********************(This is an authomatic message, please do not reply it.)******************** " + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            msg += "CONGRATULATIONS ! Your transaction is successfuly done." + Environment.NewLine + Environment.NewLine;
            msg += "These are the buyer's info for contacts:" + Environment.NewLine+Environment.NewLine+Environment.NewLine;
            msg += "-NAME....:" + buyer.UserName + " " + buyer.MiddleName + " " + buyer.LastName+Environment.NewLine;
            msg += "-EMAIL...:" + buyer.Email+Environment.NewLine;
            msg += "-PHONE...:" + buyer.Phone+Environment.NewLine;
            msg += Environment.NewLine + Environment.NewLine + Environment.NewLine;
            msg += "Purchase information :"+Environment.NewLine;
            msg += "=>ORDER # .........:" + order.OrderId.ToString()+Environment.NewLine;
            msg += "=>Product ID.......:" + book.BookId.ToString()+Environment.NewLine;
            msg += "=>Title............:" + book.Title.ToString() + Environment.NewLine;
            msg += "=>Study Area.......:" + book.StudyArea.StudyAreaName.ToString();
            msg += Environment.NewLine;
            msg += "=>Book Condition...:" + book.BookCondition.Condition.ToString()+Environment.NewLine;
            msg += "=>Quantity.........:" + order.Quantity+Environment.NewLine;
            msg += "=>Price............:" + order.Price+Environment.NewLine;
            msg += "----------------------------------------------"+Environment.NewLine;
            msg += "=>Total............:" + order.Quantity * order.Price+Environment.NewLine;
            return msg;

        }
    }
}