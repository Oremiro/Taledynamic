using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class StateAuth:State
    {
        public override void Auth(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "You already logging...."
            );
        }

        public override void SendingData(ITelegramBotClient botClient, Update update)
        {
            //реализация посылки сообщений, пока что в душе не чаю как
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Start sending...."
            );
        }

        public override void StopSendingData(ITelegramBotClient botClient,Update update)
        {
            this._user.ChangeState(new StateStopped());
        }
    }
}