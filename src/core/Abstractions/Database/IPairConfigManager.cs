using core.Infrastructure.Database.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IPairConfigManager : IDatabaseManager<PairConfig>
	{
		Task<IEnumerable<PairConfig>> GetAssignedPairs (long ExchangeCode, IDbConnection connection, IDbTransaction transaction = null);
	}
}
