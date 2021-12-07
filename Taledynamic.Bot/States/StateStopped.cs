using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class StateStopped:State
    {

        public override void Auth(ITelegramBotClient botClient, Update update)
        { 
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вы уже авторизованы."
            );
        }

        public override void SendingData(ITelegramBotClient botClient, Message message)
        {
            
             botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Продолжаю обработку..."
            );
             this._user.ChangeState(new StateMessageHandling());
        }

        public override void StopSendingData(ITelegramBotClient botClient,Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вы уже остановили обработку сообщений."
            );
        }

        public override async void DefaultAction(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Вы можете продолжить обработку с помощью команды /sending ."
            );
        }
    }
}