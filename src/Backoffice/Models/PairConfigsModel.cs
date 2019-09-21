using core.Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Backoffice.Models
{
	public class PairConfigsModel
	{
		public string ExchangeName { get;  set; }
		public long ExchangeCode { get; set; }

		public List<PairConfig> Configs { get; set; } = new List<PairConfig>();

		public List<SelectListItem> AllAssets { get; set; } = new List<SelectListItem>();
	}
}
