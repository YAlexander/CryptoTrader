using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;
using Persistence.Helpers;

namespace Persistence.Managers
{
	public interface ICandlesManager : IBaseDatabaseManager<Candle>
	{
		Task<IEnumerable<Candle>> Get(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles, IDbConnection connection, IDbTransaction transaction = null);
	}
}