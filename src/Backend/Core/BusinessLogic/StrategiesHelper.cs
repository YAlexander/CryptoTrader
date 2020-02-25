using System;
using System.Collections.Generic;
using Contracts.Trading;
using TechanCore.Strategies;
using TechanCore.Strategies.Options;

namespace Core.BusinessLogic
{
	public class StrategiesHelper
	{
		public ITradingStrategy<IStrategyOption> Get(string code, IStrategyOption options)
		{
			if (_strategies.ContainsKey(code))
			{
				return _strategies[code](options);
			}
			
			throw new Exception($"Unknown strategy {code}");
		}

		readonly Dictionary<string, Func<IStrategyOption, ITradingStrategy<IStrategyOption>>> _strategies = new Dictionary<string, Func<IStrategyOption, ITradingStrategy<IStrategyOption>>>
		{
			[nameof(AwesomeMacdStrategy)] = options => new AwesomeMacdStrategy((AwesomeMacdStrategyOptions)options),
			[nameof(AwesomeSmaStrategy)] = options => new AwesomeSmaStrategy((AwesomeSmaStrategyOptions)options),
			[nameof(Base150Strategy)] = options => new Base150Strategy((Base150StrategyOptions)options),
			[nameof(BigThreeStrategy)] = options => new BigThreeStrategy((BigTreeStrategyOptions)options),
			[nameof(BollingerAwesomeMacdStrategy)] = options => new BollingerAwesomeMacdStrategy((BollingerAwesomeMacdStrategyOptions)options),
			[nameof(BollingerBandsRsiStrategy)] = options => new BollingerBandsRsiStrategy((BollingerBandsRsiStrategyOptions)options),
			[nameof(MacdCrossStrategy)] = options => new MacdCrossStrategy((MacdCrossStrategyOptions)options),
			[nameof(MacdSmaStrategy)] = options => new MacdSmaStrategy((MacdSmaStrategyOptions)options),		
			[nameof(MacdTemaStrategy)] = options => new MacdTemaStrategy((MacdTemaStrategyOptions)options),
			[nameof(MomentumStrategy)] = options => new MomentumStrategy((MomentumStrategyOptions)options),
			[nameof(QuickSmaStrategy)] = options => new QuickSmaStrategy((QuickSmaStrategyOptions)options),
			[nameof(RsiMacdStrategy)] = options => new RsiMacdStrategy((RsiMacdStrategyOptions)options),
			[nameof(TripleMaStrategy)] = options => new TripleMaStrategy((TripleMaStrategyOptions)options)
		};
	}
}
