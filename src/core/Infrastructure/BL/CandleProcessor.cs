using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class CandleProcessor : BaseProcessor
	{
		private ICandleManager _candleManager;

		public CandleProcessor (
			IOptions<AppSettings> settings,
			ILogger<BaseProcessor> logger,
			ICandleManager candleManager) : base(settings, logger)
		{
			_candleManager = candleManager;
		}

		public Task<long> Create(Candle candle)
		{
			return WithConnection((connection, transaction) =>
			{
				return _candleManager.Create(candle, connection, transaction);
			});
		}
	}
}
