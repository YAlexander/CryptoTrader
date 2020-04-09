using System.Data;
using System.Threading.Tasks;
using Abstractions.Enums;
using Persistence.Entities;

namespace Persistence.Managers
{
	public interface IDealsManager : IDatabaseManager
	{
		Task<Deal> Get(Exchanges exchange, Assets asset1, Assets asset2, IDbConnection connection, IDbTransaction transaction = null);
		Task<Deal> Update(Deal deal, IDbConnection connection, IDbTransaction transaction = null);
	}
}