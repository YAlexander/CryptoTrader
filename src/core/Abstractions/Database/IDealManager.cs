using core.Infrastructure.Database.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IDealManager : IDatabaseManager<Deal>
	{
		Task<IEnumerable<Deal>> GetOpenDeals (int exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null);
		Task<IEnumerable<Order>> GetAssociatedOrders (long dealId, IDbConnection connection, IDbTransaction transaction = null);
	}
}
