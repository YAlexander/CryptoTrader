using System;

namespace core.Abstractions
{
	public interface ICandle
	{
		public long Id { get; set; }
		DateTime Time { get; set; }
		decimal High { get; set; }
		decimal Low { get; set; }
		decimal Open { get; set; }
		decimal Close { get; set; }
		decimal Volume { get; set; }
	}
}
