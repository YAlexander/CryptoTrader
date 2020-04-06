using System.Threading.Tasks;
using Abstractions.Entities;
using Orleans;

namespace Abstractions.Grains
{
    public interface IStrategyGrain : IGrainWithIntegerCompoundKey
    {
        Task<IStrategyInfo> Get();
    }
}