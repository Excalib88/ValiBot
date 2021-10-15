using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ValiBot.Entities;
using ValiBot.Services;

namespace ValiBot.Commands
{
    public class GetOperationsCommand : BaseCommand
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;

        public GetOperationsCommand(DataContext context, IUserService userService, TelegramBot telegramBot)
        {
            _context = context;
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.GetOperationsCommand;
        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);

            var operations = _context.Operations.Include(x => x.Category)
                .Where(x => x.IsFinished && x.UserId == user.Id).ToList();
            var debits = operations.Where(x => x.Type == OperationType.Debit).ToList();
            var credits = operations.Where(x => x.Type == OperationType.Credit).ToList();

            var message = new StringBuilder("Ваши операции: \n" +
                                            "Доходы: \n");

            foreach (var operation in credits)
            {
                message.AppendLine($"{operation.Name} : {operation.Price} : {operation.CreatedAt}");
            }

            message.AppendLine("Расходы:");
            
            foreach (var operation in debits)
            {
                message.AppendLine($"{operation.Name} : {operation.Price} : {operation.CreatedAt}");
            }

            await _botClient.SendTextMessageAsync(user.ChatId, message.ToString(), ParseMode.Markdown);
        }
    }
}