using Common;

namespace Binance
{
	public class AppSettings
	{
		public string ApiKey { get; set; } 
		public string ApiSecret { get; set; }
		
		public DatabaseOptions DatabaseOptions { get; set; }
	} 
}