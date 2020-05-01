using System;
using TechanCore.Analytics.Options;
using TechanCore.Analytics.Result;

namespace TechanCore.Analytics
{
	public class AlphaRatio : IAnalyzer<AlphaRatioOptions, DefaultAnalyticsResult>
	{
		public DefaultAnalyticsResult Get(AlphaRatioOptions options)
		{
			//Коэффициент Альфа(Alpha)

			//Коэффициент Альфа был разработан Майклом Дженсеном(Michael C. Jensen) для сравнения доходности портфеля относительно рынка. Другими словами, он позволяет понять, превосходит ли портфель рынок или от него отстает.Рассчитывается Alpha как:

			//Alpha = Rp – (Rf + B * (Rm – Rf)), где
			//	Rp – Return of Portfolio – доходность портфеля фонда;
			//Rf – Risk - Free Rate of Return – безрисковая ставка на рынке; *
			//	В – коэффициент Бета портфеля фонда;
			//Rm – Return of Market – доходность эталонного портфеля.
			

			//* Для оценки безрисковой доходности на рынке обычно берется доходность казначейских векселей США(Treasury bills).

			//Чем выше значение коэффициента Альфа, тем лучше. Отрицательная Alpha может говорить как о плохом управлении портфелем, так и высоком коэффициенте затрат(например, в результате слишком частой балансировки).

			//Что касается портфелей индексных ETF, то наличие у них отрицательной Alpha(по логике она должна быть равна нулю) связано с расходами фонда: биржевой фонд отстает от динамики индекса на сумму затрат.

			throw new NotImplementedException();
		}
	}
}
