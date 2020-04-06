namespace Abstractions.Entities
{
	public interface IFill
	{
		decimal Price { get; set; }
		decimal Quantity { get; set; }
		decimal Fee { get; set; }
		string FeeAsset { get; set; }
		long TradeId { get; set; }
	}
}
