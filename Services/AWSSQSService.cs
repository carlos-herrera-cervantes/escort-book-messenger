using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using EscortBookMessenger.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EscortBookMessenger.Services
{
    public class AWSSQSService : IAWSSQSService
    {
        #region snippet_Properties

        private readonly string _queueUrl;

        private readonly IAmazonSQS _sqsClient;

        #endregion

        #region snippet_Constructors

        public AWSSQSService(IConfiguration configuration)
        {
            var sqsSection = configuration.GetSection("AWS").GetSection("SQS");
            var accessKey = sqsSection.GetSection("AccessKey").Value;
            var secretKey = sqsSection.GetSection("SecretKey").Value;

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            _queueUrl = sqsSection.GetSection("QueueUrl").Value;
            _sqsClient = new AmazonSQSClient(credentials, new AmazonSQSConfig
            {
                ServiceURL = sqsSection.GetSection("Endpoint").Value
            });
        }

        #endregion

        #region snippet_ActionMethods

        public async Task<IEnumerable<RequestorsMessage>> GetMessageAsync()
        {
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = 10
            };
            var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);

            if (!response.Messages.Any()) return null;

            var requestorsMessages = new List<RequestorsMessage>();

            foreach (var message in response.Messages)
            {
                var requestorsMessage = JsonConvert.DeserializeObject<RequestorsMessage>(message.Body);
                requestorsMessages.Add(requestorsMessage);

                await DeleteMessageAsync(message);
            }

            return requestorsMessages;
        }

        public async Task DeleteMessageAsync(Message message)
            => await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);

        #endregion
    }
}