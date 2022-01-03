using System.Threading.Tasks;
using EscortBookMessenger.Models;

namespace EscortBookMessenger.Services
{
    public interface IMessenger
    {
        Task SendEmailAsync(RequestorsMessage requestorsMessage);
    }
}