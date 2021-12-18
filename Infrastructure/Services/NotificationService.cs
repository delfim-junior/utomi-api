using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Application.Dto;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class NotificationService : INotification
    {
        public async Task<NotificationResponse> SendEmail(List<string> recipients, string body, string subject)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("delfim.uu.junior@gmail.com", "p@ged0wn"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            mailMessage.Subject = $"UTOMI App - {subject}";
            mailMessage.To.Add(string.Join(',', recipients));
            mailMessage.From = new MailAddress("delfim.uu.junior@gmail.com");

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return new NotificationResponse
                {
                    Message = "Message Sent",
                    Success = true
                };
            }
            catch (Exception e)
            {
                return new NotificationResponse
                {
                    Message = e.Message,
                    Success = false
                };
            }
        }
    }
}