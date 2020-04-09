using System.Data;
using System.Threading.Tasks;
using Abstractions.Enums;
using Dapper;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class DealsManager : IDealsManager
	{
		public Task<Deal> Get(Exchanges exchange, Assets asset1, Assets asset2, IDbConnection connection, IDbTransaction transaction = null)
		{
			DealStatus status = DealStatus.OPEN;
			string query = $@"
				select * from {exchange}.Deals where asset1 = @asset1 and asset2 = @asset2 and dealStatus = @dealStatus
			";

			return connection.QueryFirstOrDefaultAsync<Deal>(query, 
				new
				{
					asset1 = asset1,
					asset2 = asset2,
					dealStatus = status
				}, transaction);
		}

		public Task<Deal> Update(Deal deal, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}
	}
}