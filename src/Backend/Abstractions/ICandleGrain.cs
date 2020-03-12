using System.Threading.Tasks;
using Contracts;

namespace Abstractions
{
    public interface ICandleGrain
    {
        Task Set(ICandle candle);
        Task<ICandle> Get();
    }
}