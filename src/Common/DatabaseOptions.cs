namespace Common
{
	public class DatabaseOptions
	{
		public bool MigrateDatabaseOnStart { get; set; }
		public string SystemConnectionString { get; set; }
		public string CryptoTradingConnectionString { get; set; }
	}
}