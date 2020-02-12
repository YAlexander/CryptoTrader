namespace Contracts.Trading
{
	public interface IIndicator
	{
		string Name { get; }

		dynamic Get (ICandle[] candles, IOptionsSet options);

		dynamic Get (decimal?[] candles, IOptionsSet options);
	}
}
