using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class OrdersManager : IOrdersManager
	{
		public Task<Order> Create(Order order, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}

		public Task<Order> Update(Order order, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}

		public Task<IEnumerable<Order>> GetDealOrders(long dealId, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}
	}
}