using System.Data;
using System.Threading.Tasks;
using Contracts.Enums;

namespace Persistence.Helpers
{
	public interface IBaseDatabaseManager<T> where T : new()
	{
		Task<T> Create(T obj, IDbConnection connection, IDbTransaction transaction = null);
		
		Task<T> Update(T obj, IDbConnection connection, IDbTransaction transaction = null);
	}
}