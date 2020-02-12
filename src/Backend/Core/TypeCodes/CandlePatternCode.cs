using Contracts.TypeCodes;

namespace Core.TypeCodes
{
	public class CandlePatternCode : TypeCodeBase<int, CandlePatternCode>, ICandlePatternCode
	{
		private CandlePatternCode (int code, string description) : base(code, description)
		{
		}

		public static CandlePatternCode Unknown { get; } = new CandlePatternCode(0, "UNKNOWN");

		public static CandlePatternCode Doji { get; } = new CandlePatternCode(1, "DOJI");

		public static CandlePatternCode ShootingStar { get; } = new CandlePatternCode(2, "SHOOTING_STAR");

		public static CandlePatternCode BearishEveningStar { get; } = new CandlePatternCode(3, "BEARISH_EVENING_STAR");

		public static CandlePatternCode BullishMorningStar { get; } = new CandlePatternCode(4, "BULLISH_MORNING_STAR");

		public static CandlePatternCode BullishHammer { get; } = new CandlePatternCode(5, "BULLISH_HAMMER");

		public static CandlePatternCode BearishInvertedHammer { get; } = new CandlePatternCode(6, "BEARISH_INVERTED_HAMMER");

		public static CandlePatternCode BearishHarami { get; } = new CandlePatternCode(7, "BEARISH_HARAMI");

		public static CandlePatternCode BullishHarami { get; } = new CandlePatternCode(8, "BULLISH_HARAMI");

		public static CandlePatternCode BearishEngulfing { get; } = new CandlePatternCode(9, "BEARISH_ENGULFING");

		public static CandlePatternCode BullishEngulfing { get; } = new CandlePatternCode(10, "BULLISH_ENGULFING");

		public static CandlePatternCode PiercingLine { get; } = new CandlePatternCode(11, "PIERCING_LINE");

		public static CandlePatternCode BullishBelt { get; } = new CandlePatternCode(12, "BULLISH_BELT");

		public static CandlePatternCode BullishKicker { get; } = new CandlePatternCode(13, "BULLISH_KICKER");

		public static CandlePatternCode BearishKicker { get; } = new CandlePatternCode(14, "BEARISH_KICKER");

		public static CandlePatternCode BearishHangingMan { get; } = new CandlePatternCode(14, "BEARISH_HANGING_MAN");

		public static CandlePatternCode BearishDarkCloudCover { get; } = new CandlePatternCode(15, "BEARISH_DARK_CLOUD_COVER");

		public static CandlePatternCode BullishMarubozu { get; } = new CandlePatternCode(16, "BULLISH_MARUBOZU");

		public static CandlePatternCode BearishMarubozu { get; } = new CandlePatternCode(17, "BEARISH_MARUBOZU");
	}
}
