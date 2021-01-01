using System.Threading.Tasks;

namespace RpaUi.Interfaces
{
    public interface IMyEmailSender
    {
        Task SendEmail(string email, string subject, string htmlMessage, string htmlAttachment = "",string filename = "");
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, string attachment = "", string filename = "");
    }
}