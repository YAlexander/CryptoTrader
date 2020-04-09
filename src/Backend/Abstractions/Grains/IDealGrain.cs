using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
	public interface IDealGrain : IGrainWithIntegerCompoundKey
	{
		Task<IDeal> Get();
		Task Create(IDeal deal);
	}
}