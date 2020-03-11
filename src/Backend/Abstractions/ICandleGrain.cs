using System.Threading.Tasks;
using Contracts;

namespace Abstractions
{
    public interface ICandleGrain
    {
        Task<ICandle> Update(ICandle candle);
        Task<ICandle> Get();
    }
}