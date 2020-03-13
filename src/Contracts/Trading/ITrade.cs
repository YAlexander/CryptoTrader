namespace Contracts.Trading
{
	public interface ITrade
	{
		long OrderId { get; set; }
		
		decimal Price { get; set; }
		decimal Quantity { get; set; }
		decimal Fee { get; set; }
		string FeeAsset { get; set; }
		long TradeId { get; set; }
	}
}