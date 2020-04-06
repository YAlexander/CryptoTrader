using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
	public interface ITradeProcessingGrain : IGrainWithIntegerCompoundKey
	{
		Task Set(ITrade trade);
	}
}