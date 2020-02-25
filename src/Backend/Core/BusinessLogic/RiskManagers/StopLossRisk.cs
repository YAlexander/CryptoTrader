using System.Threading.Tasks;
using Contracts;
using Contracts.Trading;

namespace Core.BusinessLogic.RiskManagers
{
    public class StopLossRiskManager : IRisk<decimal?>
    {
        public async Task<decimal?> Get(ICandle[] candles, IStrategyInfo info, decimal[] balances)
        {
            throw new System.NotImplementedException();
        }
    }
}