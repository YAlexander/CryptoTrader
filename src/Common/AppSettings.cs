using Common;

namespace Silo
{
	public class AppSettings
	{
		public int DefaultTimeoutSeconds { get; set; }
		public string NatsHost { get; set; }
		public int NatsPort { get; set; }
		public string DashboardUser { get; set; }
		public string DashboardPassword { get; set; }
		
		public string ClusterId { get; set; }
		public string ServiceId { get; set; }
		public string AdvertisedIPAddress { get; set; }
		public DatabaseOptions DatabaseOptions { get; set; }
	}
}