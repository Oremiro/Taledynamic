using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class State_Auth:IState
    {
        public override void Auth(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "You already logging...."
            );
        }

        public override void Sending_Data(ITelegramBotClient botClient, Update update)
        {
            //реализация посылки сообщений, пока что в душе не чаю как
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Start sending...."
            );
        }

        public override void Stop_sending_Data(ITelegramBotClient botClient,Update update)
        {
            this._user.Change_State(new State_Stopped());
        }
    }
}