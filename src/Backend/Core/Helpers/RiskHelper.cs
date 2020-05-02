using System;
using Abstractions;
using Core.BusinessLogic.RiskManagement;

namespace Core.Helpers
{
	public static class RiskHelper
	{
		public static IRiskStrategy Get(string strategy, string options)
		{
			return strategy switch
			{
				nameof(SimpleRiskStrategy) => new SimpleRiskStrategy(options),
				_ => throw new Exception($"Unknown Risk Manager: {strategy}")
			};
		}
	}
}