using Binance.Net.Objects;
using core.Infrastructure.Database.Entities;
using core.TypeCodes;

namespace CandlesWorker.Extensions
{
	public static class BinanceExtensions
	{
		public static Trade ToEntity (this BinanceStreamTrade item)
		{
			Trade trade = new Trade();
			trade.ExchangeCode = ExchangeCode.BINANCE.Code;
			trade.Symbol = item.Symbol;
			trade.Time = item.TradeTime;
			trade.TradeId = item.TradeId;
			trade.Price = item.Price;
			trade.Quantity = item.Quantity;

			return trade;
		}
	}
}
