using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
	public interface IOrderGrain : IGrainWithGuidCompoundKey
	{
		Task Receive(IOrder order);
		Task Update(IOrder order);
	}
}