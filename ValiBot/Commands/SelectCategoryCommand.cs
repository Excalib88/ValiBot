using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ValiBot.Entities;
using ValiBot.Services;

namespace ValiBot.Commands
{
    public class SelectCategoryCommand : BaseCommand
    {
        private readonly TelegramBotClient _botClient;
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public SelectCategoryCommand(TelegramBot telegramBot, DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
            _botClient = telegramBot.GetBot().Result;
        }

        public override string Name => CommandNames.SelectCategoryCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var priceAndName = update.Message.Text.Split(':');
            var user = await _userService.GetOrCreate(update);

            var operation = new Operation
            {
                Name = priceAndName[1],
                UserId = user.Id
            };
            
            if (priceAndName[0].IndexOf('-') != -1)
            {
                operation.Price = decimal.Parse(priceAndName[0].Remove(priceAndName[0].IndexOf('-'), 1));
                operation.Type = OperationType.Debit;
            }
            else
            {
                operation.Price = decimal.Parse(priceAndName[0]);
                operation.Type = OperationType.Credit;
            }

            await _context.Operations.AddAsync(operation);
            await _context.SaveChangesAsync();
            
            var categories = _context.Categories.Where(x => x.UserId == user.Id && x.Type == operation.Type).ToList();
            var message = new StringBuilder("Операция добавлена, осталось выбрать категорию! \n"+
                                            $"Тип операции:{operation.Type.ToString()}\n"+
                                            $"Сумма:{operation.Price}\n");

            foreach (var category in categories)
            {
                message.AppendLine($"{category.Id} : {category.Name}");
            }

            message.AppendLine("Выберите категорию отправив сообщение с номером категории");
            
            await _botClient.SendTextMessageAsync(update.Message.Chat.Id, message.ToString(), ParseMode.Markdown);
        }
    }
}