using System.Net.Http;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class StateStopped:State
    {
        private static readonly HttpClient client = new HttpClient();

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
            await botclient.AnswerCallbackQueryAsync(callbackQuery.Id, "Success");
            /*var response = await client.GetAsync("auth/User/send"); //get запрос
 
             var responseString = await response.Content.ReadAsStringAsync();
             
             if (responseString == "Success")
             {
                 await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Success");
             }
             else 
            {
                this._user.ChangeState(new StateNonAuth());
            }*/
        }
    }
}