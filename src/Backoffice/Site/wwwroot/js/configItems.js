'use strict'

class TradingPair {
	constructor(asset1, asset2, strategy) {
		this.asset1 = asset1;
		this.asset2 = asset2;
		this.strategy = strategy;
	}
}

class StrategySettings {
	constructor(code) {
		this.code = code;
	}
}

class AdxMasStrategyOptions {
	constructor(maType, fastMaPeriod, slowMaPeriod, adxPeriod) {
		this.maType = maType;
		this.fastMaPeriod = fastMaPeriod;
		this.slowMaPeriod = slowMaPeriod;
		this.adxPeriod = adxPeriod;
	}
}

class AwesomeMacdStrategyOptions {
	constructor(macdFastPeriod, macdSlowPeriod, macdSignalPeriod, awesomeFastPeriod, awesomeSlowPeriod) {
		this.macdFastPeriod = macdFastPeriod;
		this.macdSlowPeriod = macdSlowPeriod;
		this.macdSignalPeriod = macdSignalPeriod;
		this.awesomeFastPeriod = awesomeFastPeriod;
		this.awesomeSlowPeriod = awesomeSlowPeriod;
	}
}

class AwesomeSmaStrategyOptions {
	constructor(awesomeFastPeriod, awesomeSlowPeriod, maType, maFastPeriod, maSlowPeriod) {
		this.awesomeFastPeriod = awesomeFastPeriod;
		this.awesomeSlowPeriod = awesomeSlowPeriod;
		this.maType = maType;
		this.maFastPeriod = maFastPeriod;
		this.maSlowPeriod = maSlowPeriod;
	}
}

class Base150StrategyOptions {
	constructor(veryFastSmaPeriod, fastSmaPeriod, slowSmaPeriod, verySlowSmaPeriod, priceToUse) {
		this.veryFastSmaPeriod = veryFastSmaPeriod;
		this.fastSmaPeriod = fastSmaPeriod;
		this.slowSmaPeriod = slowSmaPeriod;
		this.verySlowSmaPeriod = verySlowSmaPeriod;
		this.priceToUse = priceToUse;
	}
}

class BigTreeStrategyOptions {
	constructor(veryFastSmaPeriod, fastSmaPeriod, slowSmaPeriod) {
		this.veryFastSmaPeriod = veryFastSmaPeriod;
		this.fastSmaPeriod = fastSmaPeriod;
		this.slowSmaPeriod = slowSmaPeriod;
	}
}

class BollingerAwesomeMacdStrategyOptions {
	constructor(
		fastPeriod,
		slowPeriod,
		signalPeriod,
		bollingerPeriod,
		bollingerDeviationDown,
		bollingerDeviationUp,
		awesomeFastPeriod,
		awesomeSlowPeriod,
		emaPeriod,
		smaFastPeriod,
		smaSlowPeriod) {
		this.fastPeriod = fastPeriod;
		this.slowPeriod = slowPeriod;
		this.signalPeriod = signalPeriod;
		this.bollingerPeriod = bollingerPeriod;
		this.bollingerDeviationDown = bollingerDeviationDown;
		this.bollingerDeviationUp = bollingerDeviationUp;
		this.awesomeFastPeriod = awesomeFastPeriod;
		this.awesomeSlowPeriod = awesomeSlowPeriod;
		this.emaPeriod = emaPeriod;
		this.smaFastPeriod = smaFastPeriod;
		this.smaSlowPeriod = smaSlowPeriod;
	}
}

class BollingerBandsRsiStrategyOptions {
	constructor(bollingerPeriod, bollingerDeviationDown, bollingerDeviationUp, rsiPeriod) {
		this.bollingerPeriod = bollingerPeriod;
		this.bollingerDeviationDown = bollingerDeviationDown;
		this.bollingerDeviationUp = bollingerDeviationUp;
		this.rsiPeriod = rsiPeriod;
	}
}

class BreakoutMaOptions {
	constructor(smaPeriod, emaPeriod, adxPeriod) {
		this.smaPeriod = smaPeriod;
		this.emaPeriod = emaPeriod;
		this.adxPeriod = adxPeriod;
	}
}

class BullishEngulfingOptions {
	constructor(rsiOptions) {
		this.rsiOptions = rsiOptions;
	}
}

class CciMaStrategyOptions {
	constructor(maType, cciPeriod, maFast, maSlow) {
		this.maType = maType;
		this.cciPeriod = cciPeriod;
		this.maFast = maFast;
		this.maSlow = maSlow;
	}
}

class CciRsiOptions {
	constructor(rsiPeriod, cciPeriod) {
		this.rsiPeriod = rsiPeriod;
		this.cciPeriod = cciPeriod;
	}
}

class CciScalperStrategyOptions {
	constructor(cciPeriod, maType, fastMaPeriod, normalMaPeriod, slowMaPeriod) {
		this.cciPeriod = cciPeriod;
		this.maType = maType;
		this.fastMaPeriod = fastMaPeriod;
		this.normalMaPeriod = normalMaPeriod;
		this.slowMaPeriod = slowMaPeriod;
	}
}

class DoubleVolatilityStrategyOptions {
	constructor(maType, fastMaPeriod, normalMaPeriod, slowMaPeriod, rsiPeriod) {
		this.maType = maType;
		this.fastMaPeriod = fastMaPeriod;
		this.normalMaPeriod = normalMaPeriod;
		this.slowMaPeriod = slowMaPeriod;
		this.rsiPeriod = rsiPeriod;
	}
}

class MaAdxMacdOptions {
	constructor(macdFastPeriod, macdSlowPeriod, macdSignalPeriod, maType, fastMaPeriod, slowMaPeriod, adxPeriod) {
		this.macdFastPeriod = macdFastPeriod;
		this.macdSlowPeriod = macdSlowPeriod;
		this.macdSignalPeriod = macdSignalPeriod;
		this.maType = maType;
		this.fastMaPeriod = fastMaPeriod;
		this.slowMaPeriod = slowMaPeriod;
		this.adxPeriod = adxPeriod;
	}
}

class MaCrossStrategyOptions {
	constructor(maType, fastMaPeriod, slowMaPeriod) {
		this.maType = maType;
		this.fastMaPeriod = fastMaPeriod;
		this.slowMaPeriod = slowMaPeriod;
	}
}

class FifthElementStrategyOptions {
	constructor(fastPeriod, slowPeriod, signalPeriod) {
		this.fastPeriod = fastPeriod;
		this.slowPeriod = slowPeriod;
		this.signalPeriod = signalPeriod;
	}
}

class FractalsStrategyOptions {
	constructor(awesomeFastPeriod, awesomeSlowPeriod, exitAfterBarsCount) {
		this.awesomeFastPeriod = awesomeFastPeriod;
		this.awesomeSlowPeriod = awesomeSlowPeriod;
		this.exitAfterBarsCount = exitAfterBarsCount;
	}
}

class HeikenAshiStrategyOptions {
	constructor(maType, maPeriod) {
		this.maType = maType;
		this.maPeriod = maPeriod;
	}
}

class MacdCrossStrategyOptions {
	constructor(fastPeriod, slowPeriod, signalPeriod) {
		this.fastPeriod = fastPeriod;
		this.slowPeriod = slowPeriod;
		this.signalPeriod = signalPeriod;
	}
}

class MacdMaStrategyOptions {
	constructor(macdFastPeriod, macdSlowPeriod, macdSignalPeriod, maType, fastMaPeriod, slowMaPeriod, normalMaPeriod) {
		this.macdFastPeriod = macdFastPeriod;
		this.macdSlowPeriod = macdSlowPeriod;
		this.macdSignalPeriod = macdSignalPeriod;
		this.maType = maType;
		this.fastMaPeriod = fastMaPeriod;
		this.slowMaPeriod = slowMaPeriod;
		this.normalMaPeriod = normalMaPeriod;
	}
}

class MacdTemaStrategyOptions {
	constructor(macdFastPeriod, macdSlowPeriod, macdSignalPeriod, temaPeriod) {
		this.macdFastPeriod = macdFastPeriod;
		this.macdSlowPeriod = macdSlowPeriod;
		this.macdSignalPeriod = macdSignalPeriod;
		this.temaPeriod = temaPeriod;
	}
}

class MomentumStrategyOptions {
	constructor(maType, fastMaPeriod, slowMaPeriod, momentumPeriod, rsiPeriod) {
		this.maType = maType;
		this.fastMaPeriod = fastMaPeriod;
		this.slowMaPeriod = slowMaPeriod;
		this.momentumPeriod = momentumPeriod;
		this.rsiPeriod = rsiPeriod;
	}
}

class QuickSmaStrategyOptions {
	constructor(fastSmaPeriod, slowSmaPeriod) {
		this.fastSmaPeriod = fastSmaPeriod;
		this.slowSmaPeriod = slowSmaPeriod;
	}
}

class RsiAwesomeMfiStrategyOptions {
	constructor(rsiPeriod, mfiPeriod, awesomeFastPeriod, awesomeSlowPeriod) {
		this.rsiPeriod = rsiPeriod;
		this.mfiPeriod = mfiPeriod;
		this.awesomeFastPeriod = awesomeFastPeriod;
		this.awesomeSlowPeriod = awesomeSlowPeriod;
	}
}

class RsiMacdAwesomeStrategyOptions {
	constructor(macdFastPeriod, macdSlowPeriod, macdSignalPeriod, rsiPeriod, awesomeFastPeriod, awesomeSlowPeriod) {
		this.macdFastPeriod = macdFastPeriod;
		this.macdSlowPeriod = macdSlowPeriod;
		this.macdSignalPeriod = macdSignalPeriod;
		this.rsiPeriod = rsiPeriod;
		this.awesomeFastPeriod = awesomeFastPeriod;
		this.awesomeSlowPeriod = awesomeSlowPeriod;
	}
}

class RsiMacdStrategyOptions {
	constructor(macdFastSmaPeriod, macdSlowSmaPeriod, macdSignalPeriod, rsiPeriod) {
		this.macdFastSmaPeriod = macdFastSmaPeriod;
		this.macdSlowSmaPeriod = macdSlowSmaPeriod;
		this.macdSignalPeriod = macdSignalPeriod;
		this.rsiPeriod = rsiPeriod;
	}
}

class RsiMaCrossoverStrategyOptions {
	constructor(maType, maFastPeriod, maSlowPeriod, rsiPeriod) {
		this.maType = maType;
		this.maFastPeriod = maFastPeriod;
		this.maSlowPeriod = maSlowPeriod;
		this.rsiPeriod = rsiPeriod;
	}
}

class MaStochRsiStrategyOptions {
	constructor(stochPeriod, stochEmaPeriod, maType, maPeriod, rsiPeriod) {
		this.stochPeriod = stochPeriod;
		this.stochEmaPeriod = stochEmaPeriod;
		this.maType = maType;
		this.maPeriod = maPeriod;
		this.rsiPeriod = rsiPeriod;
	}
}

class StochAdxStrategyOptions {
	constructor(stochPeriod, stochEmaPeriod, adxPeriod) {
		this.stochPeriod = stochPeriod;
		this.stochEmaPeriod = stochEmaPeriod;
		this.adxPeriod = adxPeriod;
	}
}

class StochRsiMacdStrategyOptions {
	constructor(stochPeriod, stochEmaPeriod, macdFastSmaPeriod, macdSlowSmaPeriod, macdSignalPeriod) {
		this.stochPeriod = stochPeriod;
		this.stochEmaPeriod = stochEmaPeriod;
		this.macdFastSmaPeriod = macdFastSmaPeriod;
		this.macdSlowSmaPeriod = macdSlowSmaPeriod;
		this.macdSignalPeriod = macdSignalPeriod;
	}
}

class ThreeMAgosStrategyOptions {
	constructor(smaPeriod, emaPeriod, wmaPeriod) {
		this.smaPeriod = smaPeriod;
		this.emaPeriod = emaPeriod;
		this.wmaPeriod = wmaPeriod;
	}
}

class TripleMaStrategyOptions {
	constructor(fastSmaPeriod, slowSmaPeriod, emaPeriod) {
		this.fastSmaPeriod = fastSmaPeriod;
		this.slowSmaPeriod = slowSmaPeriod;
		this.emaPeriod = emaPeriod;
	}
}