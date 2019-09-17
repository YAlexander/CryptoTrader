using core.Infrastructure.Database.Entities;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IBalanceManager : IDatabaseManager<Balance>
	{
		Task<Balance> Get (string asset, long exchangeCode, IDbConnection connection, IDbTransaction transaction = null);
	}
}
