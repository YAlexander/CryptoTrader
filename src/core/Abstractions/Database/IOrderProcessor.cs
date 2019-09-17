using core.Infrastructure.Database.Entities;
using core.Infrastructure.Models;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IOrderProcessor
	{
		Task<ExchangeOrder> List (long orderId);

		Task<long?> Create (Order order);

		public Task<Order> Update (Order order);

		public Task<Order> Update (ExchangeOrder order);
	}
}
