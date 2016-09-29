using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Helpers
{
    public class EmailClient
    {
        //sifra ne moze ovdje biti nikako !!!
        private const string _host = "smtp-mail.outlook.com";
        private const string _senderEmail = "UPPHercegovina@outlook.com";
        private const string _password = "eT27AFGQ";
        private const string _displayName = "Udruženje poljuprivrednih proizvođača Hercegovina";
        private MailMessage _mailMessage;
        private string _recipientEmail;
      
        public EmailClient(string subject, string recipientEmail, string body)
        {
            _mailMessage = new MailMessage(
                new MailAddress(_senderEmail, _displayName),
                new MailAddress(recipientEmail)); 
        
            _mailMessage.Subject = subject;
            _recipientEmail = recipientEmail; 
            _mailMessage.Body = body;
            _mailMessage.IsBodyHtml = true;
        }

        public void Send()
        {
            SmtpClient smtp = new SmtpClient(_host);
            smtp.Credentials = new NetworkCredential(_senderEmail, _password);
            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.Send(_mailMessage);
        }
    }
}