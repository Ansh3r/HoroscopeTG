using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using HtmlAgilityPack;

namespace Tgbot
{
    class Program
    {
        private static string Token { get; set; } = "1991055510:AAEWN018nJpA39t_9QzvnBj3yF4mCXKJu4E";
        private static TelegramBotClient client;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(Token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();
        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if (msg.Text != null)
            {
                var web = new HtmlWeb();
                var htmlDoc = web.Load("https://my-calend.ru/goroskop");
                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='goroskop-items-description']");

                Console.WriteLine($"Пришло сообщение с текстом: {msg.Text}");

                foreach (HtmlNode node in nodes)
                {
                    var horo = node.Element("div");
                    var name_horo = node.Element("a");
                    var text_horo = node.Element("div");

                    if (msg.Text == name_horo.InnerText)
                    {
                        var text = text_horo.InnerText.Replace("&ndash;", "-");
                        await client.SendTextMessageAsync(msg.Chat.Id, text);
                        break;
                    }
                }
                await client.SendTextMessageAsync(msg.Chat.Id, "Выберите знак зодиака: ", replyMarkup: GetButtons());
            }
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Овен"}, new KeyboardButton { Text = "Телец"} },
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Близнецы"}, new KeyboardButton { Text = "Рак"} },
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Лев"}, new KeyboardButton { Text = "Дева"} },
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Весы"}, new KeyboardButton { Text = "Скорпион"} },
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Стрелец"}, new KeyboardButton { Text = "Козерог"} },
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Водолей"}, new KeyboardButton { Text = "Рыбы"} }
                }
            };
        }
    }
}
