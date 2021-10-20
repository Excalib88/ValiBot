using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ValiBot.Services;

namespace ValiBot.Commands
{
    public class GetAnalyticsCommand : BaseCommand
    {
        private readonly IUserService _userService;
        private readonly IAnalyticService _analyticService;
        private readonly TelegramBotClient _telegramBotClient;

        public GetAnalyticsCommand(IUserService userService, IAnalyticService analyticService, TelegramBot telegramBot)
        {
            _userService = userService;
            _analyticService = analyticService;
            _telegramBotClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.GetAnalyticsCommand;
        public override async Task ExecuteAsync(Update update)
        {
            var user = await _userService.GetOrCreate(update);
            var daysString = update.CallbackQuery?.Data?.Replace("analytic-", "") ?? "0";
            var days = int.Parse(daysString);
            var analyticData = await _analyticService.GetAnalytic(update, days);

            await _telegramBotClient.SendTextMessageAsync(user.ChatId, analyticData, ParseMode.Markdown);
        }
    }
}