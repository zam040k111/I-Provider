using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace I_Provider.BLL.Identity
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Подключите здесь службу SMS, чтобы отправить текстовое сообщение.
            return Task.FromResult(0);
        }
    }
}
