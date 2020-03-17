using System;
using System.Collections.Generic;
using Abstractions;
using Common.Trading;
using TechanCore;
using TechanCore.Strategies;
using TechanCore.Strategies.Options;

namespace Core.BusinessLogic
{
	public static class StrategiesHelper
	{
		public static ITradingStrategy<IStrategyOption> Get(string code, IStrategyOption options)
		{
			if (!Strategies.ContainsKey(code))
			{
				throw new Exception($"Unknown strategy {code}");
			}
			
			return Strategies[code](options);
		}

		private static readonly Dictionary<string, Func<IStrategyOption, ITradingStrategy<IStrategyOption>>> Strategies = new Dictionary<string, Func<IStrategyOption, ITradingStrategy<IStrategyOption>>>
		{
			[nameof(AdxEmasStrategy)] = options => new AdxEmasStrategy((AdxEmasStrategyOptions)options),
			[nameof(AdxSmasStrategy)] = options => new AdxSmasStrategy((AdxSmasStrategyOptions)options),
			[nameof(AwesomeMacdStrategy)] = options => new AwesomeMacdStrategy((AwesomeMacdStrategyOptions)options),
			[nameof(AwesomeSmaStrategy)] = options => new AwesomeSmaStrategy((AwesomeSmaStrategyOptions)options),
			[nameof(Base150Strategy)] = options => new Base150Strategy((Base150StrategyOptions)options),
			[nameof(BigThreeStrategy)] = options => new BigThreeStrategy((BigTreeStrategyOptions)options),
			[nameof(BollingerAwesomeMacdStrategy)] = options => new BollingerAwesomeMacdStrategy((BollingerAwesomeMacdStrategyOptions)options),
			[nameof(BollingerBandsRsiStrategy)] = options => new BollingerBandsRsiStrategy((BollingerBandsRsiStrategyOptions)options),
			[nameof(BreakoutMaStrategy)] = options => new BreakoutMaStrategy((BreakoutMaOptions)options),
			[nameof(BullishEngulfingStrategy)] = options => new BullishEngulfingStrategy((BullishEngulfingOptions)options),
			[nameof(BuyAndHoldStrategy)] = options => new BuyAndHoldStrategy((EmptyStrategyOptions)options),
			[nameof(CciEmaStrategy)] = options => new CciEmaStrategy((CciEmaStrategyOptions)options),
			[nameof(CciRsiStrategy)] = options => new CciRsiStrategy((CciRsiOptions)options),
			[nameof(CciScalperStrategy)] = options => new CciScalperStrategy((CciScalperStrategyOptions)options),
			[nameof(DoubleVolatilityStrategy)] = options => new DoubleVolatilityStrategy((DoubleVolatilityStrategyOptions)options),
			[nameof(EmaAdxMacdStrategy)] = options => new EmaAdxMacdStrategy((EmaAdxMacdOptions)options),
			[nameof(EmaCrossStrategy)] = options => new EmaCrossStrategy((EmaCrossStrategyOptions)options),
			[nameof(FifthElementStrategy)] = options => new FifthElementStrategy((FifthElementStrategyOptions)options),
			[nameof(FractalsStrategy)] = options => new FractalsStrategy((FractalsStrategyOptions)options),
			[nameof(MacdCrossStrategy)] = options => new MacdCrossStrategy((MacdCrossStrategyOptions)options),
			[nameof(MacdSmaStrategy)] = options => new MacdSmaStrategy((MacdSmaStrategyOptions)options),
			[nameof(MacdTemaStrategy)] = options => new MacdTemaStrategy((MacdTemaStrategyOptions)options),
			[nameof(MomentumStrategy)] = options => new MomentumStrategy((MomentumStrategyOptions)options),
			[nameof(QuickSmaStrategy)] = options => new QuickSmaStrategy((QuickSmaStrategyOptions)options),
			[nameof(RsiAwesomeMfiStrategy)] = options => new RsiAwesomeMfiStrategy((RsiAwesomeMfiStrategyOptions)options),
			[nameof(RsiMacdAwesomeStrategy)] = options => new RsiMacdAwesomeStrategy((RsiMacdAwesomeStrategyOptions)options),
			[nameof(RsiMacdStrategy)] = options => new RsiMacdStrategy((RsiMacdStrategyOptions)options),
			[nameof(RsiSmaCrossoverStrategy)] = options => new RsiSmaCrossoverStrategy((RsiSmaCrossoverStrategyOptions)options),
			[nameof(SimpleBearBullStrategy)] = options => new SimpleBearBullStrategy((EmptyStrategyOptions)options),
			[nameof(SmaGoldenCrossStrategy)] = options => new SmaGoldenCrossStrategy((EmptyStrategyOptions)options),
			[nameof(SmaStochRsiStrategy)] = options => new SmaStochRsiStrategy((SmaStochRsiStrategyOptions)options),
			[nameof(StochAdxStrategy)] = options => new StochAdxStrategy((StochAdxStrategyOptions)options),
			[nameof(StochRsiMacdStrategy)] = options => new StochRsiMacdStrategy((StochRsiMacdStrategyOptions)options),
			[nameof(ThreeMAgosStrategy)] = options => new ThreeMAgosStrategy((ThreeMAgosStrategyOptions)options),
			[nameof(TripleMaStrategy)] = options => new TripleMaStrategy((TripleMaStrategyOptions)options),
		};
	}
}
