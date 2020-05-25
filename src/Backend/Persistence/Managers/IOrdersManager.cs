using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Persistence.Entities;

namespace Persistence.Managers
{
	public interface IOrdersManager : IDatabaseManager
	{
		Task<OrderEntity> Create(OrderEntity order, IDbConnection connection, IDbTransaction transaction = null);
		Task<OrderEntity> Update(OrderEntity order, IDbConnection connection, IDbTransaction transaction = null);
		Task<IEnumerable<OrderEntity>> GetDealOrders(long dealId, IDbConnection connection, IDbTransaction transaction = null);
	}
}