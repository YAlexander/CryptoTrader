namespace core
{
	public class AppSettings
	{
		public string ConnectionString { get; set; }
		public string BusConnectionString { get; set; }
		public string CandlesQueueName { get; set; }
		public string TradesQueueName { get; set; }
		public string OrdersQueueName { get; set; }
		public string TelegramChatId { get; set; }
		public string TelegramKey { get; set; }
		public bool SendTelegramNotifacation { get; set; }
		public bool DisadleDealsSaving { get; set; }
	}
}
