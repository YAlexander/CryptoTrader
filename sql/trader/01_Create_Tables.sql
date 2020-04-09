--Test Data

CREATE SCHEMA Binance;

CREATE TABLE Binance.ExchangeSettings (
	exchange int not null,
	asset1 int not null,
	asset2 int not null,
	timeframe int not null,
	isEnabled boolean NOT NULL,
	updatingInterval int not null
);

GRANT ALL PRIVILEGES ON TABLE Binance.ExchangeSettings to trader;

insert into Binance.ExchangeSettings (exchange, asset1, asset2, timeframe, isEnabled, updatingInterval)
values (0, 0, 7, 60, true, 10)

CREATE TABLE Binance.Candles (
	id BIGSERIAL PRIMARY KEY,
	created timestamp NOT NULL,
	updated timestamp,
	exchange int NOT NULL,
	asset1 int NOT NULL,
	asset2 int NOT NULL,
	timeFrame int NOT NULL,
	high decimal NOT NULL,
	low decimal NOT NULL,
	open decimal NOT NULL,
	close decimal NOT NULL,
	volume decimal NOT NULL,
	trades int NOT NULL,
	time timestamp NOT NULL
);

GRANT ALL PRIVILEGES ON TABLE Binance.Candles to trader;
