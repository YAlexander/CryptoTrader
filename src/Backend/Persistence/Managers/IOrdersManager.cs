using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Persistence.Entities;

namespace Persistence.Managers
{
	public interface IOrdersManager : IDatabaseManager
	{
		Task<Order> Create(Order order, IDbConnection connection, IDbTransaction transaction = null);
		Task<Order> Update(Order order, IDbConnection connection, IDbTransaction transaction = null);
		Task<IEnumerable<Order>> GetDealOrders(long dealId, IDbConnection connection, IDbTransaction transaction = null);
	}
}