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

		public async Task<long?> Create(Candle candle)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _candleManager.Create(candle, connection, transaction);
			});
		}
	}
}
