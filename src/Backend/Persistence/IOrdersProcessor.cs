using System;
using System.Threading.Tasks;
using Abstractions.Entities;
using Abstractions.Enums;

namespace Persistence
{
	public interface IOrdersProcessor
	{
		Task<IOrder> Get(Exchanges primaryKey, Assets asset1, Assets Asset2, Guid? id);
		Task<IOrder> Update(IOrder order);
	}
}