using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enums;
using Contracts.Trading;
using Persistence.Entities;

namespace Persistence.StrategyOptions
{
	public class AdxMomentumOptionsProcessor : IStrategyOption
	{
		private readonly IStrategyOptionsManager _optionsManager;
		private readonly Exchanges _exchange;
		private readonly Assets _asset1;
		private readonly Assets _asset2;
		private readonly string _strategyName;
		
		public AdxMomentumOptionsProcessor(IStrategyOptionsManager optionsManager, Exchanges exchange, Assets asset1, Assets asset2, string strategyName)
		{
			_optionsManager = optionsManager;
			_exchange = exchange;
			_asset1 = asset1;
			_asset2 = asset2;
			_strategyName = strategyName;
		}

		public T GetOptions<T>() where T : class, new()
		{
			Task<IEnumerable<StrategyOptionLine>> options = _optionsManager.GetOptions(_exchange, _asset1, _asset2, _strategyName);
		}
	}
}