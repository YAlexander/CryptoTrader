using core.Infrastructure.Database.Entities;
using core.Infrastructure.Models;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IOrderManager : IDatabaseManager<Order>
	{
		Task<Order> Update (ExchangeOrder entity, IDbConnection connection, IDbTransaction transaction = null);
	}
}
