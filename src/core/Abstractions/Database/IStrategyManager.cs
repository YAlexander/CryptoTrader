using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IStrategyManager : IDatabaseManager<Strategy>
	{
		Task<IEnumerable<Strategy>> GetAll (IDbConnection connection, IDbTransaction transaction = null);

		Task<bool> Enable (long id, IDbConnection connection, IDbTransaction transaction = null);

		Task<Strategy> Get (IExchangeCode exchangeCode, string pair, IDbConnection connection, IDbTransaction transaction = null);
	}
}
