using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Models;
using System.Threading.Tasks;

namespace core.Abstractions
{
	public interface IExchangeOrdersSender
	{
		IExchangeCode Exchange { get; }

		Task<ExchangeOrder> Send (Order order, ExchangeConfig config);
	}
}
