using System.Threading.Tasks;

namespace Outbracket.Common.Services.Notifier
{
    public interface ISender
    {
        bool Validate(Recipient recipient);
        
        Task<bool> SendRegistrationMessageAsync(Recipient recipient, string link);
        
        Task<bool> SendRestorePasswordMessageAsync(Recipient recipient, string link);
    }
}