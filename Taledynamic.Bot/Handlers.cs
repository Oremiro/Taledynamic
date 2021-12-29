using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Serilog;
using TaleDynamicBot.States;

namespace TaleDynamicBot
{
    public class Handlers
    {
        public static User user = new User(new StateNonAuth());

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Log.Error(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            try
            {
                await BotOnMessageReceived(botClient,update);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        public static async Task BotOnMessageReceived(ITelegramBotClient botClient, Update update)
        {
            Log.Information($"Receive message type: {update.Message.Type}");
            var obj =  JsonConvert.SerializeObject(update);

            using (var helper = new ApiHelper(
                botClient,
                "http://localhost:5000",
                update.Message.From.Id.ToString()))
            {
                await helper.SaveMessage(botClient, update.Message);
            }

            Log.Information(obj);
            
        }
        static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
        {
            const string usage = "Usage:\n" +
                                 "/auth     - authorize in project\n" +
                                 "/sending  - start sending message on TaleDynamic\n"+
                                 "/stop_sending - stop receiving your messages\n";

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id, 
                text: usage,
                replyMarkup: new ReplyKeyboardRemove());
        }
    }
}