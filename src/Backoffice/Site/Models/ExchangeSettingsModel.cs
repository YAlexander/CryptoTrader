using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions.Enums;

namespace Site.Models
{
	public class ExchangeSettingsModel
	{
		public Exchanges Exchange { get; set; }
		public List<KeyValuePair<int, string>> ExchangesDropdown => GetDropdownData<Exchanges>();
		public List<KeyValuePair<int, string>> AssetsDropdown => GetDropdownData<Assets>();

		public BinanceSettingsModel BinanceSettings { get; set; }
		public BitmexSettingsModel BitmexSettings { get; set; }
		public QuantfurySettingsModel QuantfurySettings { get; set; }

		private List<KeyValuePair<int, string>> GetDropdownData<T>()
		{
			List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>();

			foreach (var name in Enum.GetNames(typeof(T)))
			{
				result.Add(new KeyValuePair<int, string>((int)Enum.Parse(typeof(T), name), name));
			}

			return result;
		}
	}
}
