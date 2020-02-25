using System;
using Contracts.Enums;

namespace Contracts
{
	public interface ICandle
	{ 
		/// <summary>
		/// Candle time
		/// </summary>
		DateTime Time { get; set; }
		
		Timeframes TimeFrame { get; set; }
		
		/// <summary>
		/// High price
		/// </summary>
		decimal High { get; set; }
		
		/// <summary>
		/// Low price
		/// </summary>
		decimal Low { get; set; }
		
		/// <summary>
		/// Open price
		/// </summary>
		decimal Open { get; set; }
		
		/// <summary>
		/// Close price
		/// </summary>
		decimal Close { get; set; }
		
		/// <summary>
		/// Volume
		/// </summary>
		decimal Volume { get; set; }
		
		/// <summary>
		/// Number of trades
		/// </summary>
		decimal Trades { get; set; }
	}
}
