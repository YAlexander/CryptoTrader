using core.Abstractions.TypeCodes;

namespace core.TypeCodes
{
	public class CandlePatternCode : TypeCodeBase<int, CandlePatternCode>, ICandlePatternCode
	{
		public CandlePatternCode (int code, string description) : base(code, description)
		{
		}

		public static CandlePatternCode UNKNOWN { get; } = new CandlePatternCode(0, "UNKNOWN");

		public static CandlePatternCode DOJI { get; } = new CandlePatternCode(1, "DOJI");

		public static CandlePatternCode SHOOTING_STAR { get; } = new CandlePatternCode(2, "SHOOTING_STAR");

		public static CandlePatternCode BEARISH_EVENING_STAR { get; } = new CandlePatternCode(3, "BEARISH_EVENING_STAR");

		public static CandlePatternCode BULLISH_MORNING_STAR { get; } = new CandlePatternCode(4, "BULLISH_MORNING_STAR");

		public static CandlePatternCode BULLISH_HAMMER { get; } = new CandlePatternCode(5, "BULLISH_HAMMER");

		public static CandlePatternCode BEARISH_INVERTED_HAMMER { get; } = new CandlePatternCode(6, "BEARISH_INVERTED_HAMMER");

		public static CandlePatternCode BEARISH_HARAMI { get; } = new CandlePatternCode(7, "BEARISH_HARAMI");

		public static CandlePatternCode BULLISH_HARAMI { get; } = new CandlePatternCode(8, "BULLISH_HARAMI");

		public static CandlePatternCode BEARISH_ENGULFING { get; } = new CandlePatternCode(9, "BEARISH_ENGULFING");

		public static CandlePatternCode BULLISH_ENGULFING { get; } = new CandlePatternCode(10, "BULLISH_ENGULFING");

		public static CandlePatternCode PIERCING_LINE { get; } = new CandlePatternCode(11, "PIERCING_LINE");

		public static CandlePatternCode BULLISH_BELT { get; } = new CandlePatternCode(12, "BULLISH_BELT");

		public static CandlePatternCode BULLISH_KICKER { get; } = new CandlePatternCode(13, "BULLISH_KICKER");

		public static CandlePatternCode BEARISH_KICKER { get; } = new CandlePatternCode(14, "BEARISH_KICKER");

		public static CandlePatternCode BEARISH_HANGING_MAN { get; } = new CandlePatternCode(14, "BEARISH_HANGING_MAN");

		public static CandlePatternCode BEARISH_DARK_CLOUD_COVER { get; } = new CandlePatternCode(15, "BEARISH_DARK_CLOUD_COVER");

		public static CandlePatternCode BULLISH_MARUBOZU { get; } = new CandlePatternCode(16, "BULLISH_MARUBOZU");

		public static CandlePatternCode BEARISH_MARUBOZU { get; } = new CandlePatternCode(17, "BEARISH_MARUBOZU");
	}
}
