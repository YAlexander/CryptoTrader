using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
	public interface ITradeProcessingGrain : IGrainWithIntegerKey
	{
		Task Process(ITrade trade);
	}
}