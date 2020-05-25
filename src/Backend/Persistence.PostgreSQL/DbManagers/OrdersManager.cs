using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class OrdersManager : IOrdersManager
	{
		public Task<OrderEntity> Create(OrderEntity order, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}

		public Task<OrderEntity> Update(OrderEntity order, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<OrderEntity>> GetDealOrders(long dealId, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}
	}
}