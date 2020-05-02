namespace Abstractions
{
	public interface IRiskStrategyOptions
	{
		bool TradeOnFlat { get; set; }
		
		bool UseTrailingStop { get; set; }
		
		bool UseMarginalTrading { get; set; }
		int Leverage { get; set; }
		
		decimal MinimalOrderAmount { get; set; }
		
		decimal DefaultOpenGap { get; set; }
	}
}