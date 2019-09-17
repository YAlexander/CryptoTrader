using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions
{
	public interface IDatabaseManager<T> where T: IEntity
	{
		Task<long> Create (T entity, IDbConnection connection, IDbTransaction transaction = null);
		Task<T> Update (T entity, IDbConnection connection, IDbTransaction transaction = null);
		Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null);
		Task<T> Get (long id, IDbConnection connection, IDbTransaction transaction = null);
	}
}