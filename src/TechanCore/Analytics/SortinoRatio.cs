using System;
using TechanCore.Analytics.Options;
using TechanCore.Analytics.Result;

namespace TechanCore.Analytics
{
	public class SortinoRatio : IAnalyzer<SortinoRatioOptions, DefaultAnalyticsResult>
	{
		public DefaultAnalyticsResult Get(SortinoRatioOptions options)
		{
			//Коэффициент Сортино(Sortino Ratio)

			//Коэффициент Сортино рассчитывается аналогично коэффициенту Шарпа с тем лишь отличием, что в его формуле используются не все отклонения средней доходности фонда, а только те, которые имеют отрицательное значение.

			//	Sortino Ratio = (Portfolio return − Risk - Free Rate) / Standard Deviation of Portfolio of Negative Asset Returns
			//Коэффициент Сортино = (Доходность портфеля – Безрисковая ставка) / Стандартное отклонение отклонение доходности портфеля в отрицательную сторону
			//Чем больше значение Sortino Ratio, тем выше эффективность портфеля и тем ниже инвестиционные риски вложения в фонд.


			throw new NotImplementedException();
		}
	}
}
