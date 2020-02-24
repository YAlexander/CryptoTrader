using System.Threading.Tasks;
using Contracts;
using Contracts.Trading;

namespace Core.BusinessLogic.RiskManagers
{
    public class TakeProfitRisk : IRisk
    {
        public Task Get(ICandle[] candles, IStrategyInfo info, decimal[] balances, ref IRiskResult result)
        {
            decimal tp = 0;
            result.TakeProfitPrice = tp;

            return Task.CompletedTask;
        }
    }
}