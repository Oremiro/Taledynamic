using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class StateStopped:State
    {

        public override async Task Auth(ITelegramBotClient botClient, Message message)
        { 
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Вы уже авторизованы."
            );
        }

        public override async Task SendingData(ITelegramBotClient botClient, Message message)
        {
            
             await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Продолжаю обработку..."
            );
             this._user.ChangeState(new StateMessageHandling());
        }

        public override async Task StopSendingData(ITelegramBotClient botClient,Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Вы уже остановили обработку сообщений."
            );
        }

        public override async Task DefaultAction(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Вы можете продолжить обработку с помощью команды /sending ."
            );
        }

        public override async Task CallbackQueryHandler(ITelegramBotClient botclient, CallbackQuery callbackQuery)
        {
            Log.Information($"{callbackQuery.From}");
        }
    }
}