namespace core.Infrastructure.Database.Entities
{
	public class Strategy : BaseEntity
	{
		public string Name { get; set; }
		public int OptimalTimeframe { get; set; }
		public int NumberOfCandles { get; set; }
		public string TypeName { get; set; }
		public int Version { get; set; }
		public bool IsEnabled { get; set; }
		public string Preset { get; set; }
	}
}