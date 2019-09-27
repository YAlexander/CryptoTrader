using core.Infrastructure.Database.Entities;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface ITradesManager : IDatabaseManager<Trade>
	{
		public Task<Trade> GetLast (int exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null);
		
		Task<int> ClearData (int exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null);
	}
}
