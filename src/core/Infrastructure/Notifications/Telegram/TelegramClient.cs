using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace core.Infrastructure.Notifications.Telegram
{
	public class TelegramClient
	{
		private TelegramBotClient botClient;
		private IOptions<AppSettings> _settings;

		public TelegramClient(IOptions<AppSettings> settings)
		{
			_settings = settings;
			botClient = new TelegramBotClient(_settings.Value.TelegramKey);
			botClient.OnMessage += OnMessage;
			botClient.StartReceiving();
		}

		public async Task SendMessage (string message)
		{
			ChatId chatId = new ChatId(-1001463232032);
			await botClient.SendTextMessageAsync(chatId, message);
		}

		private void OnMessage (object sender, MessageEventArgs e)
		{
		}
	}
}
