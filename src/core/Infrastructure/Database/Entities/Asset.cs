using System.ComponentModel.DataAnnotations;

namespace core.Infrastructure.Database.Entities
{
	public class Asset : BaseEntity
	{
		public string Code { get; set; }
		public string Name { get; set; }
		
		[Display(Name ="Is Enabled")]
		public bool IsEnabled { get; set; }
	}
}
