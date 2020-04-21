using System.Text.Json;
using Abstractions;
using Abstractions.Entities;

namespace Core.BusinessLogic.RiskManagement
{
	public abstract class BaseRiskStrategy<T> : IRiskStrategy where T : IRiskStrategyOptions
	{
		private readonly string _options;

		protected BaseRiskStrategy(string options)
		{
			_options = options;
		}
		
		protected virtual T Options => JsonSerializer.Deserialize<T>(_options);
		
		public abstract string Name { get; }

		public abstract ITradingContext Process(ITradingContext context, IStrategyInfo info);
	}
}