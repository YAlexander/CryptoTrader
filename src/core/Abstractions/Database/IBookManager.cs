using core.Infrastructure.Database.Entities;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IBookManager : IDatabaseManager<Book>
	{
		Task<Book> GetLast (int exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null);
	}
}
