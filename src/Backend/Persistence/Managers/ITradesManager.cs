using System.Threading.Tasks;
using Abstractions.Enums;

namespace Persistence.Managers
{
    public interface ITradesManager : IDatabaseManager
    {
        Task<int> GetNumberOfProfitableDeals(Exchanges exchange, Assets asset1, Assets asset2);
    }
}