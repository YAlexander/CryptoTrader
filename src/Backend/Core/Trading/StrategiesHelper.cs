using System;
using System.Collections.Generic;
using Contracts.Enums;
using Contracts.Trading;
using Core.Trading.Strategies;
using Persistence;
using Persistence.StrategyOptions;

namespace core.Trading
{
	public static class StrategiesHelper
	{
		public static ITradingStrategy Get(string code)
		{
			if (Strategies.ContainsKey(code))
			{
				return Strategies[code]();
			}
			
			throw new Exception($"Unknown strategy {code}");
		}
		
		private static readonly Dictionary<string, Func<ITradingStrategy>> Strategies = new Dictionary<string, Func<ITradingStrategy>>()
		{
			[nameof(AdxMomentum)] = () => new AdxMomentum(),
			[nameof(AdxSmas)] = () => new AdxSmas(),
			[nameof(AwesomeMacd)] = () => new AwesomeMacd(),
			[nameof(AwesomeSma)] = () => new AwesomeSma(),
			[nameof(Base150)] = () => new Base150(),
			[nameof(BbandRsi)] = () => new BbandRsi(),
			[nameof(BigThree)] = () => new BigThree(),
			[nameof(BollingerAwe)] = () => new BollingerAwe(),
			[nameof(BreakoutMa)] = () => new BreakoutMa(),
			[nameof(BullishEngulfing)] = () => new BullishEngulfing(),
			[nameof(BuyAndHold)] = () => new BuyAndHold(),
			[nameof(CciEma)] = () => new CciEma(),
			[nameof(CciRsi)] = () => new CciRsi(),
			[nameof(CciScalper)] = () => new CciScalper(),
			[nameof(CloudBreakout)] = () => new CloudBreakout(),
			[nameof(DerivativeOscillator)] = () => new DerivativeOscillator(),
			[nameof(DoubleVolatility)] = () => new DoubleVolatility(),
			[nameof(EmaAdx)] = () => new EmaAdx(),
			[nameof(EmaAdxF)] = () => new EmaAdxF(),
			[nameof(EmaAdxMacd)] = () => new EmaAdxMacd(),
			[nameof(EmaAdxSmall)] = () => new EmaAdxSmall(),
			[nameof(EmaCross)] = () => new EmaCross(),
			[nameof(EmaCrossShort)] = () => new EmaCrossShort(),
			[nameof(EmaStochRsi)] = () => new EmaStochRsi(),
			[nameof(FaMaMaMa)] = () => new FaMaMaMa(),
			[nameof(FifthElement)] = () => new FifthElement(),
			[nameof(FisherTransform)] = () => new FisherTransform(),
			[nameof(Fractals)] = () => new Fractals(),
			[nameof(FreqClassic)] = () => new FreqClassic(),
			[nameof(FreqMod)] = () => new FreqMod(),
			[nameof(FreqTrade)] = () => new FreqTrade(),
			[nameof(FreqTradeEvo)] = () => new FreqTradeEvo(),
			[nameof(MacdCross)] = () => new MacdCross(),
			[nameof(MacdSma)] = () => new MacdSma(),
			[nameof(MacdTema)] = () => new MacdTema(),
			[nameof(Momentum)] = () => new Momentum(),
			[nameof(PatternTrading)] = () => new PatternTrading(),
			[nameof(PivotMaestro)] = () => new PivotMaestro(),
			[nameof(PowerRanger)] = () => new PowerRanger(),
			[nameof(QuickSma)] = () => new QuickSma(),
			[nameof(RedWedding)] = () => new RedWedding(),
			[nameof(RedWeddingWalder)] = () => new RedWeddingWalder(),
			[nameof(Replex)] = () => new Replex(),
			[nameof(RsiBbands)] = () => new RsiBbands(),
			[nameof(RsiMacd)] = () => new RsiMacd(),
			[nameof(RsiMacdAwesome)] = () => new RsiMacdAwesome(),
			[nameof(RsiMacdMfi)] = () => new RsiMacdMfi(),
			[nameof(RsiOversoldOverbought)] = () => new RsiOversoldOverbought(),
			[nameof(RsiSarAwesome)] = () => new RsiSarAwesome(),
			[nameof(SarAwesome)] = () => new SarAwesome(),
			[nameof(SarRsi)] = () => new SarRsi(),
			[nameof(SarStoch)] = () => new SarStoch(),
			[nameof(SimpleBearBull)] = () => new SimpleBearBull(),
			[nameof(SmaCrossover)] = () => new SmaCrossover(),
			[nameof(SmaGoldenCross)] = () => new SmaGoldenCross(),
			[nameof(SmaSar)] = () => new SmaSar(),
			[nameof(SmaStochRsi)] = () => new SmaStochRsi(),
			[nameof(StochAdx)] = () => new StochAdx(),
			[nameof(TheScalper)] = () => new TheScalper(),
			[nameof(ThreeMAgos)] = () => new ThreeMAgos(),
			[nameof(TripleMa)] = () => new TripleMa(),
			[nameof(Wvf)] = () => new Wvf(),
			[nameof(WvfEmaCrossover)] = () => new WvfEmaCrossover(),
			[nameof(WvfExtended)] = () => new WvfExtended(),
			[nameof(StochRsiMacd)] = () => new StochRsiMacd()
		};
		
		
	}
}
