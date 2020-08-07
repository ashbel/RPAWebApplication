using System.Threading.Tasks;

namespace RpaUi.Services
{
    public interface IMyEmailSender
    {
        Task SendEmail(string email, string subject, string htmlMessage, string htmlAttachment = "");
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, string attachment = "");
    }
}