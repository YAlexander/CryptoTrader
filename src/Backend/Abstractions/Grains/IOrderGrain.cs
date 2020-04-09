using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
	public interface IOrderGrain : IGrainWithGuidCompoundKey
	{
		Task<IOrder> Get();
		Task Update(IOrder order);
	}
}