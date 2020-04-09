using System.Data;
using System.Threading.Tasks;
using Abstractions.Enums;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class DealsManager : IDealsManager
	{
		public Task<Deal> Get(Exchanges exchange, Assets asset1, Assets asset2, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}

		public Task<Deal> Update(Deal deal, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}
	}
}