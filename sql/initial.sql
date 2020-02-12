create database trader;
create role trader login password '7z9Vpm8YLS';
grant all privileges on database trader to trader;

CREATE TABLE Candles (
	id BIGSERIAL PRIMARY KEY,
	created timestamp NOT NULL,
	symbol varchar(16) NOT NULL,
	exchangeCode int NOT NULL,
	time timestamp NOT NULL,
	periodCode int NOT NULL,
	openTime timestamp NOT NULL,
	closeTime timestamp NOT NULL,
	open decimal NOT NULL,
	close decimal NOT NULL,
	high decimal NOT NULL,
	low decimal NOT NULL,
	volume decimal NOT NULL,
	numberOfTrades int NOT NULL,
	isDeleted boolean NOT NULL
);

GRANT ALL PRIVILEGES ON TABLE Candles to trader;
GRANT ALL PRIVILEGES ON SEQUENCE candles_id_seq TO trader;


CREATE TABLE Strategies (
	id BIGSERIAL PRIMARY KEY,
	created timestamp NOT NULL,
	name varchar(512) NOT NULL,
	optimalTimeframe int NOT NULL,
	numberOfCandles int NOT NULL,
	typeName text NOT NULL,
	version int not null,
	description text null,
	isEnabled boolean not null,
	isDeleted boolean not null,
	preset jsonb null	
);

GRANT ALL PRIVILEGES ON TABLE Strategies to trader;
GRANT ALL PRIVILEGES ON SEQUENCE strategies_id_seq TO trader;

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'ADX Momentum', 60, 25, 'core.Trading.Strategies.AdxMomentum, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'ADX Smas', 60, 14, 'core.Trading.Strategies.AdxSmas, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Awesome MACD', 60, 40, 'core.Trading.Strategies.AwesomeMacd, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Awesome SMA', 60, 40, 'core.Trading.Strategies.AwesomeSma, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Base 150', 60, 365, 'core.Trading.Strategies.Base150, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'BBand RSI', 60, 20, 'core.Trading.Strategies.BbandRsi, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Big Three', 60, 100, 'core.Trading.Strategies.BigThree, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Bollinger Awe', 60, 50, 'core.Trading.Strategies.BollingerAwe, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Breakout MA', 60, 35, 'core.Trading.Strategies.BreakoutMa, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Bullish Engulfing', 60, 11, 'core.Trading.Strategies.BullishEngulfing, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Buy and Hold', 15, 20, 'core.Trading.Strategies.BuyAndHold, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'CCI EMA', 60, 30, 'core.Trading.Strategies.CciEma, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'CCI RSI', 60, 15, 'core.Trading.Strategies.CciRsi, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'CCI Scalper', 60, 14, 'core.Trading.Strategies.CciScalper, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Derivative Oscillator', 60, 20, 'core.Trading.Strategies.DerivativeOscillator, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Double Volatility', 60, 20, 'core.Trading.Strategies.DoubleVolatility, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'EMA ADX', 60, 36, 'core.Trading.Strategies.EmaAdx, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'EMA ADX F', 60, 15, 'core.Trading.Strategies.EmaAdxF, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'EMA ADX MACD', 60, 30, 'core.Trading.Strategies.EmaAdxMacd, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'EMA ADX Small', 60, 15, 'core.Trading.Strategies.EmaAdxSmall, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'EMA Cross', 60, 36, 'core.Trading.Strategies.EmaCross, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'EMA Cross Short', 5, 36, 'core.Trading.Strategies.EmaCrossShort, core', 1, '', true, false, null);

INSERT INTO Strategies (id ,created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'EMA Stoch RSI', 60, 36, 'core.Trading.Strategies.EmaStochRsi, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'FAMAMAMA', 60, 20, 'core.Trading.Strategies.FaMaMaMa, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), '5th Element', 60, 30, 'core.Trading.Strategies.FifthElement, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Fisher Transform', 60, 40, 'core.Trading.Strategies.FisherTransform, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Fractals', 60, 40, 'core.Trading.Strategies.Fractals, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Freq Classic', 60, 100, 'core.Trading.Strategies.FreqClassic, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Freq Modded', 60, 100, 'core.Trading.Strategies.FreqMod, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'FreqTrade', 15, 40, 'core.Trading.Strategies.FreqTrade, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Freq Trade Evo', 15, 40, 'core.Trading.Strategies.FreqTradeEvo, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'MACD X', 60, 50, 'core.Trading.Strategies.MacdCross, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'MACD SMA', 60, 200, 'core.Trading.Strategies.MacdSma, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'MACD TEMA', 60, 50, 'core.Trading.Strategies.MacdTema, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Momentum', 60, 30, 'core.Trading.Strategies.Momentum, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Pattern Trading', 60, 80, 'core.Trading.Strategies.PatternTrading, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Pivot Maestro', 15, 10, 'core.Trading.Strategies.PivotMaestro, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Power Ranger', 60, 10, 'core.Trading.Strategies.PowerRanger, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Quick SMA', 1, 20, 'core.Trading.Strategies.QuickSma, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Red Wedding', 60, 100, 'core.Trading.Strategies.RedWedding, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Red Wedding Walder', 60, 240, 'core.Trading.Strategies.RedWeddingWalder, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Replex', 60, 20, 'core.Trading.Strategies.Replex, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'RSI Bbands', 60, 200, 'core.Trading.Strategies.RsiBbands, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'RSI MACD', 60, 52, 'core.Trading.Strategies.RsiMacd, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'RSI MACD Awesome', 60, 35, 'core.Trading.Strategies.RsiMacdAwesome, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'RSI MACD MFI', 60, 35, 'core.Trading.Strategies.RsiMacdMfi, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'RSI Oversold/Overbought', 60, 200, 'core.Trading.Strategies.RsiOversoldOverbought, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'RSI SAR Awesome', 60, 35, 'core.Trading.Strategies.RsiSarAwesome, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'SAR Awesome', 60, 35, 'core.Trading.Strategies.SarAwesome, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'SAR RSI', 60, 15, 'core.Trading.Strategies.SarRsi, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'SAR Stoch', 60, 15, 'core.Trading.Strategies.SarStoch, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'The Bull and The Bear', 60, 5, 'core.Trading.Strategies.SimpleBearBull, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'SMA Crossover', 60, 60, 'core.Trading.Strategies.SmaCrossover, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'SMA 50/200 Golden Cross', 60, 200, 'core.Trading.Strategies.SmaGoldenCross, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'SMA SAR', 60, 60, 'core.Trading.Strategies.SmaSar, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'SMA Stoch RSI', 60, 150, 'core.Trading.Strategies.SmaStochRsi, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Stoch ADX', 60, 15, 'core.Trading.Strategies.StochAdx, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'The Scalper', 60, 200, 'core.Trading.Strategies.TheScalper, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Three MAgos', 60, 15, 'core.Trading.Strategies.ThreeMAgos, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Triple MA', 60, 50, 'core.Trading.Strategies.TripleMa, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Williams Vix Fix', 60, 40, 'core.Trading.Strategies.Wvf, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Wvf Ema Crossover', 15, 40, 'core.Trading.Strategies.WvfEmaCrossover, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'Williams Vix Fix (Extended)', 60, 40, 'core.Trading.Strategies.WvfExtended, core', 1, '', true, false, null);

INSERT INTO Strategies (id, created, name, optimalTimeframe, numberOfCandles, typeName, version, description, isEnabled, isDeleted, preset) 
VALUES (default, now(), 'MACD Stoch RSI', 60, 99, 'core.Trading.Strategies.StochRsiMacd, core', 1, '', true, false, null);
