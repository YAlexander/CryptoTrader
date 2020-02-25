using core.Abstractions;

namespace core.Trading.Indicators.Options
{
	public class BbandsOptions : IIndicatorOptions
	{
		public int Period { get; }
		public double DevUp { get; }
		public double DevDown { get; }
		public TicTacTec.TA.Library.Core.MAType Type { get; }

		public BbandsOptions (int period, double devUp, double devDown, TicTacTec.TA.Library.Core.MAType type)
		{
			Period = period;
			DevUp = devDown;
			DevDown = devDown;
			Type = type;
		}

		public dynamic Options => this;
	}
}
