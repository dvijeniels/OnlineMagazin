using OnlineMagazin.Models;
using System.Threading.Tasks;

namespace OnlineMagazin.Service
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);
        Task SendOrderDetailEmail(UserEmailOptions userEmailOptions);
        Task SendEmailConfirmation(UserEmailOptions userEmailOptions);
        Task SendForgotPassword(UserEmailOptions userEmailOptions);
    }
}