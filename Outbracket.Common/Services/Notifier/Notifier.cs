using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Outbracket.Common.Services.Notifier
{
    public class Notifier
    {
        private readonly ISender _currentNotifier;

        public Notifier(ISender notifier)
        {
            _currentNotifier = notifier;
        }

        public async Task<bool[]> SendRegistrationMessagesAsync(IEnumerable<Recipient> recipients, string link)
        {
            var tasks = recipients
                .Select(recipient => _currentNotifier.Validate(recipient) ?
                    _currentNotifier.SendRegistrationMessageAsync(recipient, link) : Task.FromResult(false))
                .ToArray();
            return await Task.WhenAll(tasks);
        }

        public async Task<bool[]> SendRestorePasswordMessagesAsync(IEnumerable<Recipient> recipients, string link)
        {
            var tasks = recipients
                .Select(recipient => _currentNotifier.Validate(recipient) ?
                    _currentNotifier.SendRestorePasswordMessageAsync(recipient, link) : Task.FromResult(false))
                .ToArray();
            return await Task.WhenAll(tasks);
        }
    }
}