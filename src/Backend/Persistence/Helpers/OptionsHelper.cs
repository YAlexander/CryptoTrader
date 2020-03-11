using System;
using System.Collections.Generic;
using Contracts.Trading;
using TechanCore.Strategies;
using TechanCore.Strategies.Options;

namespace Persistence.Helpers
{
	public static class OptionsHelper
	{
		public static (IStrategyOption options, IStrategyOption defaultOptions) Decode(IStrategyInfo info)
		{
			if (!Options.ContainsKey(info.Class))
			{
				throw new Exception($"Strategy {info.StrategyName} is not supported");
			}	
			
			return Options[info.Class](info.Options, info.DefaultOptions);
		}

		private static readonly Dictionary<string, Func<string, string, (IStrategyOption options, IStrategyOption defaultOptions)>> Options = new Dictionary<string, Func<string, string, (IStrategyOption options, IStrategyOption defaultOptions)>>
		{
			[nameof(AdxEmasStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<AdxSmasStrategyOptions> helper = new JsonHelper<AdxSmasStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},
			
			[nameof(AdxSmasStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<AdxSmasStrategyOptions> helper = new JsonHelper<AdxSmasStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},
			
			[nameof(AwesomeMacdStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<AwesomeMacdStrategyOptions> helper = new JsonHelper<AwesomeMacdStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(AwesomeSmaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<AwesomeSmaStrategyOptions> helper = new JsonHelper<AwesomeSmaStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(Base150Strategy)] = (options, defaultOptions) =>
			{
				JsonHelper<Base150StrategyOptions> helper = new JsonHelper<Base150StrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(BigThreeStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<BigTreeStrategyOptions> helper = new JsonHelper<BigTreeStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(BollingerAwesomeMacdStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<BollingerAwesomeMacdStrategyOptions> helper = new JsonHelper<BollingerAwesomeMacdStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(BollingerBandsRsiStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<BollingerBandsRsiStrategyOptions> helper = new JsonHelper<BollingerBandsRsiStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(BreakoutMaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<BreakoutMaOptions> helper = new JsonHelper<BreakoutMaOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(BullishEngulfingStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<BullishEngulfingOptions> helper = new JsonHelper<BullishEngulfingOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(BuyAndHoldStrategy)] = (options, defaultOptions) => (new EmptyStrategyOptions(), new EmptyStrategyOptions()),

			[nameof(CciEmaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<CciEmaStrategyOptions> helper = new JsonHelper<CciEmaStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(CciRsiStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<CciRsiOptions> helper = new JsonHelper<CciRsiOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(CciScalperStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<CciScalperStrategyOptions> helper = new JsonHelper<CciScalperStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(DoubleVolatilityStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<DoubleVolatilityStrategyOptions> helper = new JsonHelper<DoubleVolatilityStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(EmaAdxMacdStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<EmaAdxMacdOptions> helper = new JsonHelper<EmaAdxMacdOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(EmaCrossStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<EmaCrossStrategyOptions> helper = new JsonHelper<EmaCrossStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},
			
			[nameof(FifthElementStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<FifthElementStrategyOptions> helper = new JsonHelper<FifthElementStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(FractalsStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<FractalsStrategyOptions> helper = new JsonHelper<FractalsStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(MacdCrossStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<MacdCrossStrategyOptions> helper = new JsonHelper<MacdCrossStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(MacdSmaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<MacdSmaStrategyOptions> helper = new JsonHelper<MacdSmaStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(MacdTemaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<MacdTemaStrategyOptions> helper = new JsonHelper<MacdTemaStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(MomentumStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<MomentumStrategyOptions> helper = new JsonHelper<MomentumStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(QuickSmaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<QuickSmaStrategyOptions> helper = new JsonHelper<QuickSmaStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(RsiAwesomeMfiStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<RsiAwesomeMfiStrategyOptions> helper = new JsonHelper<RsiAwesomeMfiStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},
			
			[nameof(RsiMacdAwesomeStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<RsiMacdAwesomeStrategyOptions> helper = new JsonHelper<RsiMacdAwesomeStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(RsiMacdStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<RsiMacdStrategyOptions> helper = new JsonHelper<RsiMacdStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(RsiSmaCrossoverStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<RsiSmaCrossoverStrategyOptions> helper = new JsonHelper<RsiSmaCrossoverStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(SimpleBearBullStrategy)] = (options, defaultOptions) => (new EmptyStrategyOptions(), new EmptyStrategyOptions()),

			[nameof(SmaGoldenCrossStrategy)] = (options, defaultOptions) => (new EmptyStrategyOptions(), new EmptyStrategyOptions()),

			[nameof(SmaStochRsiStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<SmaStochRsiStrategyOptions> helper = new JsonHelper<SmaStochRsiStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},
			
			[nameof(StochAdxStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<StochAdxStrategyOptions> helper = new JsonHelper<StochAdxStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(StochRsiMacdStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<StochRsiMacdStrategyOptions> helper = new JsonHelper<StochRsiMacdStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(ThreeMAgosStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<ThreeMAgosStrategyOptions> helper = new JsonHelper<ThreeMAgosStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(TripleMaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<TripleMaStrategyOptions> helper = new JsonHelper<TripleMaStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			}
		};
	}
}