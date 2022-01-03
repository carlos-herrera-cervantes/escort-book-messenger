using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using EscortBookMessenger.Models;

namespace EscortBookMessenger.Services
{
    public interface IAWSSQSService
    {
         public Task<IEnumerable<RequestorsMessage>> GetMessageAsync();

         public Task DeleteMessageAsync(Message message);
    }
}