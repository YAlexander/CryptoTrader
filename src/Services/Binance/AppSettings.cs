using Common;

namespace Binance
{
	public class AppSettings
	{
		public string NatsHost { get; set; }
		public int NatsPort { get; set; }
		public string ApiKey { get; set; } 
		public string ApiSecret { get; set; }
		
		public DatabaseOptions DatabaseOptions { get; set; }
	} 
}