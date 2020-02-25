using System.Threading.Tasks;
using Contracts;
using Contracts.Trading;

namespace Core.BusinessLogic.RiskManagers
{
    public class TakeProfitRisk : IRisk<decimal?>
    {
        public async Task<decimal?> Get(ICandle[] candles, IStrategyInfo info, decimal[] balances)
        {
            decimal? tp = 0;

            return tp;
        }
    }
}