using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ValiBot.Services;

namespace ValiBot.Commands
{
    public class StartCommand : BaseCommand
    {
        private readonly IUserService _userService;
        private readonly TelegramBotClient _botClient;

        public StartCommand(IUserService userService, TelegramBot telegramBot)
        {
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.StartCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                new InlineKeyboardButton
                {
                    Text = "Создать операцию",
                    CallbackData = CommandNames.AddOperationCommand
                },
                new InlineKeyboardButton
                {
                    Text = "Получить операции",
                    CallbackData = CommandNames.GetOperationsCommand
                }
            });

            await _botClient.SendTextMessageAsync(user.ChatId, "Добро пожаловать! Я буду вести учёт ваших доходов и расходов! ", 
                ParseMode.Markdown, replyMarkup:inlineKeyboard);
        }
    }
}