﻿using core.Abstractions;

namespace Core.Trading.Indicators.Options
{
	public class PivotHighOptions : IIndicatorOptions
	{
		public int BarsLeft { get; }
		public int BarsRight { get; }
		public bool FillNullValues { get; }

		public dynamic Options => this;

		public PivotHighOptions (int barsLeft, int barsRight, bool fillNullValues)
		{
			BarsLeft = barsLeft;
			BarsRight = barsRight;
			FillNullValues = fillNullValues;
		}
	}
}
