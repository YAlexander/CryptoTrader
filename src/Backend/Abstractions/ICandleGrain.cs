using System.Threading.Tasks;
using Contracts;
using Orleans;

namespace Abstractions
{
    public interface ICandleGrain : IGrainWithIntegerCompoundKey
    {
        Task Set(ICandle candle);
        Task<ICandle> Get();
    }
}