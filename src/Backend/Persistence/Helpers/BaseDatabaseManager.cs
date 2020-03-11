using System.Data;
using System.Threading.Tasks;
using Contracts.Enums;

namespace Persistence.Helpers
{
	public interface IBaseDatabaseManager<T> where T : new()
	{
		Task<T> Get(Exchanges exchange, Assets asset1, Assets asset2, IDbConnection connection, IDbTransaction transaction = null);

		Task<T> Create(T obj, IDbConnection connection, IDbTransaction transaction = null);
		
		Task<T> Update(T obj, IDbConnection connection, IDbTransaction transaction = null);
	}
}