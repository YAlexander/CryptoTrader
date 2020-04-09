using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
	public interface IOrderProcessingGrain : INotificationService, IGrainWithIntegerKey
	{
		Task Update(IOrder order);
	}
}