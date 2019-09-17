namespace core.Infrastructure.Database.Entities
{
	public class Asset : BaseEntity
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public bool IsEnabled { get; set; }
	}
}
