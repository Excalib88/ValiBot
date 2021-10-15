using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ValiBot.Services
{
    public interface ICommandExecutor
    {
        Task Execute(Update update);
    }
}