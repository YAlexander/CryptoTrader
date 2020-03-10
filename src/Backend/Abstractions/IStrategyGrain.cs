using System.Threading.Tasks;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
    public interface IStrategyGrain : IGrainWithIntegerCompoundKey
    {
        Task<IStrategyInfo> Get();
    }
}