using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto;

namespace Application.Interfaces
{
    public interface INotification
    {
        Task<NotificationResponse> SendEmail(List<string> recipients, string body, string subject);
    }
}