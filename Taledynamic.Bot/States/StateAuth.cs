using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaleDynamicBot.States
{
    public class StateAuth:State
    {
        public override async Task Auth(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Вы уже авторизованы. Чтобы начать обработку сообщений, введите команду /sending ."
            );
        }

        public override async Task SendingData(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Начинаю обработку..."
            );
            this._user.ChangeState(new StateMessageHandling());
        }

        public override async Task StopSendingData(ITelegramBotClient botClient,Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Вы ещё не начали обработку сообщений."
            );
        }

        public override async Task  DefaultAction(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Чтобы начать обработку сообщений, введите команду /sending ."
            );
        }

        public override async Task CallbackQueryHandler(ITelegramBotClient botclient, CallbackQuery callbackQuery)
        {
            Log.Information($"{callbackQuery.From}");
        }
    }
}