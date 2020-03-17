using System.Threading.Tasks;
using Common.Trading;
using Orleans;

namespace Abstractions
{
    public interface IStrategyGrain : IGrainWithIntegerCompoundKey
    {
        Task<IStrategyInfo> Get();
    }
}