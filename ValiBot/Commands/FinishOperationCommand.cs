using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ValiBot.Services;

namespace ValiBot.Commands
{
    public class FinishOperationCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly IUserService _userService;
        private readonly IOperationService _operationService;
        private readonly DataContext _context;
        
        public FinishOperationCommand(IUserService userService, TelegramBot telegramBot, IOperationService operationService, DataContext context)
        {
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
            _operationService = operationService;
            _context = context;
        }

        public override string Name => CommandNames.FinishOperationCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            var operation = await _operationService.GetLast(user.Id);
            operation.IsFinished = true;
            operation.CategoryId = long.Parse(update.Message.Text);
            
            await _context.SaveChangesAsync();
            await _botClient.SendTextMessageAsync(user.ChatId, "Операция добавлена!", ParseMode.Markdown);
        }
    }
}