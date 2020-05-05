using System;
using System.Collections.Generic;
using Abstractions;
using Abstractions.Entities;
using TechanCore;
using TechanCore.Strategies;
using TechanCore.Strategies.Options;

namespace Persistence.Helpers
{
	public static class OptionsHelper
	{
		public static (IStrategyOption options, IStrategyOption defaultOptions) Decode(IStrategyInfo info)
		{
			if (!Options.ContainsKey(info.StrategyClass))
			{
				throw new Exception($"Strategy {info.StrategyName} is not supported");
			}	
			
			return Options[info.StrategyClass](info.Options, info.DefaultOptions);
		}

		private static readonly Dictionary<string, Func<string, string, (IStrategyOption options, IStrategyOption defaultOptions)>> Options = new Dictionary<string, Func<string, string, (IStrategyOption options, IStrategyOption defaultOptions)>>
		{
			[nameof(AdxMasStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<AdxMasStrategyOptions> helper = new JsonHelper<AdxMasStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},
						
			[nameof(AwesomeMacdStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<AwesomeMacdStrategyOptions> helper = new JsonHelper<AwesomeMacdStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(AwesomeMaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<AwesomeMaStrategyOptions> helper = new JsonHelper<AwesomeMaStrategyOptions>();
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

			[nameof(CciMaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<CciMaStrategyOptions> helper = new JsonHelper<CciMaStrategyOptions>();
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

			[nameof(MaAdxMacdStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<MaAdxMacdOptions> helper = new JsonHelper<MaAdxMacdOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(MaCrossStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<MaCrossStrategyOptions> helper = new JsonHelper<MaCrossStrategyOptions>();
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

			[nameof(MacdMaStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<MacdMaStrategyOptions> helper = new JsonHelper<MacdMaStrategyOptions>();
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
				JsonHelper<RsiMaCrossoverStrategyOptions> helper = new JsonHelper<RsiMaCrossoverStrategyOptions>();
				return (helper.FromJson(options), helper.FromJson(defaultOptions));
			},

			[nameof(SimpleBearBullStrategy)] = (options, defaultOptions) => (new EmptyStrategyOptions(), new EmptyStrategyOptions()),

			[nameof(SmaGoldenCrossStrategy)] = (options, defaultOptions) => (new EmptyStrategyOptions(), new EmptyStrategyOptions()),

			[nameof(MaStochRsiStrategy)] = (options, defaultOptions) =>
			{
				JsonHelper<MaStochRsiStrategyOptions> helper = new JsonHelper<MaStochRsiStrategyOptions>();
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