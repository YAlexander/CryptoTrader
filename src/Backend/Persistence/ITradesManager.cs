using System.Threading.Tasks;
using Contracts.Enums;

namespace Persistence
{
    public interface ITradesManager
    {
        Task<int> GetNumberOfProfitableDeals(Exchanges exchange, Assets asset1, Assets asset2);
    }
}