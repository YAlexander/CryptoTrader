using System.Threading.Tasks;
using Orleans;

namespace Abstractions
{
	public interface ITradeProcessingGrain : IGrainWithIntegerCompoundKey
	{
		Task Set(ITrade trade);
	}
}