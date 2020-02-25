using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface ICandleManager : IDatabaseManager<Candle>
	{
		Task<IEnumerable<Candle>> GetLastCandles (int exchange, string symbol, int interval, int numberOfCandles, IDbConnection connection, IDbTransaction transaction = null);
	}
}
