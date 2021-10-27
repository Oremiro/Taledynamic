using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Serilog;
using TaleDynamicBot.States;
using Telegram.Bot.Types.ReplyMarkups;

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
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static async Task BotOnMessageReceived(ITelegramBotClient botClient, Update update)
        {
            Log.Information($"Receive message type: {update.Message.Type}");
            
            if (update.Message.Type != MessageType.Text)
                return;

            switch (update.Message.Text) //KEK)))))))))))))))
            {
                case "/auth":
                    user.Auth(botClient, update);
                    break;
                case "/sending":
                    user.SendingData(botClient, update);
                    break;
                case "/stop_sending":
                    user.StopSendingData(botClient, update);
                    break;
                case "/keyboard":
                    await SendReplyKeyboard(botClient,update.Message);
                    break;
                case "/remove":
                    await RemoveKeyboard(botClient, update.Message);
                    break;
                default:
                    Usage(botClient, update.Message);
                    break;
            }

            static async Task<Message> SendReplyKeyboard(ITelegramBotClient botClient, Message message)
            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] { "Авторизация         ", "Обработка сообщений  " },
                        new KeyboardButton[] { "Остановить обработку", "Здесь какая-то кнопка" },
                    })
                {
                    ResizeKeyboard = true
                };

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: replyKeyboardMarkup);
            }
            
            static async Task<Message> RemoveKeyboard(ITelegramBotClient botClient, Message message)
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                    text: "Removing keyboard",
                    replyMarkup: new ReplyKeyboardRemove());
            }
            
            static async void Usage(ITelegramBotClient botClient, Message message)
            {
                const string usage = "Usage:\n" +
                                     "/auth     - authorize in project\n" +
                                     "/sending  - start sending message on TaleDynamic\n"+
                                     "/stop_sending - stop receiving your messages\n"+
                                     "/keyboard - send custom keyboard\n" +
                                     "/remove   - remove custom keyboard\n";

                await botClient.SendTextMessageAsync(chatId: message.Chat.Id, 
                      text: usage,
                      replyMarkup: new ReplyKeyboardRemove());
            }
        
            
            
            Log.Information($"The message was sent with id: {update.Message.MessageId}");
        }
    }
}