using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class ExchangeCode : TypeCodeBase<int, ExchangeCode>, IExchangeCode
	{
		public ExchangeCode (int code, string description) : base(code, description)
		{
		}

		/// <summary>
		/// Unsupported exchange code
		/// </summary>
		public static ExchangeCode UNKNOWN { get; } = new ExchangeCode(0, "UNKNOWN");

		public static ExchangeCode BINANCE { get; } = new ExchangeCode(1, "BINANCE");

		public static ExchangeCode BITFINEX { get; } = new ExchangeCode(2, "BITFINEX");

		public static ExchangeCode BITTREX { get; } = new ExchangeCode(3, "BITTREX");

		public static ExchangeCode POLONIEX { get; } = new ExchangeCode(4, "POLONIEX");

		public static ExchangeCode HUOBI { get; } = new ExchangeCode(5, "HUOBI");

		public static ExchangeCode HITBTC { get; } = new ExchangeCode(6, "HITBTC");

		public static ExchangeCode COINBASE { get; } = new ExchangeCode(7, "COINBASE");

		public static ExchangeCode OKEX { get; } = new ExchangeCode(8, "OKEX");

		public static ExchangeCode CRYPTOPIA { get; } = new ExchangeCode(9, "CRYPTOPIA");

		public static ExchangeCode KUCOIN { get; } = new ExchangeCode(10, "KUCOIN");
	}
}
