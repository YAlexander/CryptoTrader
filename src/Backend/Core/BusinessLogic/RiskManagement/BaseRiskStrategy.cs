using System.Linq;
using System.Text.Json;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;

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

		protected bool HasAsset(Assets asset, ITradingContext context)
		{
			IBalance balance = context.Funds.FirstOrDefault(x => x.Asset == asset);

			decimal estimatedValue;
				
			if (context.Deal.Position == DealPositions.LONG)
			{
				estimatedValue = balance != null ? balance.TotalAmount - balance.LockedAmount : 0;
			}
			else
			{
				estimatedValue = balance != null && context.LastTrade.HasValue 
					? (balance.TotalAmount - balance.LockedAmount) / context.LastTrade.Value 
					: 0;
			}
			
			return estimatedValue >= Options.MinimalOrderAmount;
		}
	}
}