using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ValiBot.Services;

namespace ValiBot.Commands
{
    public class SelectAnalyticDaysCommand : BaseCommand
    {
        private readonly TelegramBotClient _telegramBotClient;
        private readonly IUserService _userService;

        public SelectAnalyticDaysCommand(TelegramBot telegramBot, IUserService userService)
        {
            _telegramBotClient = telegramBot.GetBot().Result;
            _userService = userService;
        }

        public override string Name => CommandNames.SelectAnalyticDaysCommand;
        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);

            var inlineKeyboard = new InlineKeyboardMarkup(new []
            {
                new [] 
                {
                    new InlineKeyboardButton{Text = "Аналитика за 1", CallbackData = "analytic-1"},          
                    new InlineKeyboardButton{Text = "Аналитика за 7", CallbackData = "analytic-7"},                                                                
                    new InlineKeyboardButton{Text = "Аналитика за 14", CallbackData = "analytic-14"},
                },
                new [] 
                {
                    new InlineKeyboardButton{Text = "Аналитика за 30", CallbackData = "analytic-30"},          
                    new InlineKeyboardButton{Text = "Аналитика за 90", CallbackData = "analytic-90"},                                                                
                    new InlineKeyboardButton{Text = "Аналитика за 365", CallbackData = "analytic-365"},
                }
            });

            await _telegramBotClient.SendTextMessageAsync(user.ChatId, "Выберите количество дней за которые нужна аналитика", 
                ParseMode.Markdown, replyMarkup:inlineKeyboard);
        }
    }
}