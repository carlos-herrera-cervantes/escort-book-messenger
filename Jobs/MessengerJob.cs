using System.Linq;
using System.Threading.Tasks;
using Coravel.Invocable;
using EscortBookMessenger.Services;

namespace EscortBookMessenger.Jobs
{
    public class MessengerJob : IInvocable
    {
        #region snippet_Properties

        private readonly IMessenger _messenger;

        private readonly IAWSSQSService _sqsService;

        #endregion

        #region snippet_Constructors

        public MessengerJob(IMessenger messenger, IAWSSQSService sqsService)
            => (_messenger, _sqsService) = (messenger, sqsService);

        #endregion

        #region snippet_ActionMethods

        public async Task Invoke()
        {
            var requestorsMessages = await _sqsService.GetMessageAsync();

            if (requestorsMessages is null || requestorsMessages.Count() == 0) return;
                
            var tasks = requestorsMessages.Select(message => _messenger.SendEmailAsync(message));

            await Task.WhenAll(tasks);
        }

        #endregion
    }
}