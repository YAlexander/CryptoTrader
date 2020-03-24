using Common;

namespace Silo
{
	public class AppSettings
	{
		public string ClusterId { get; set; }
		public string ServiceId { get; set; }
		
		public string DashboardUser { get; set; }
		public string DashboardPassword { get; set; }
		
		public string ClusterInvariant { get; set; } 

		public DatabaseOptions DatabaseOptions { get; set; }
	}
}