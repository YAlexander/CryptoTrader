using System.Threading.Tasks;
using Contracts;
using Contracts.Trading;

namespace Core.BusinessLogic.RiskManagers
{
    public class StopLossRiskManager : IRisk
    {
        public Task Get(ICandle[] candles, IStrategyInfo info, decimal[] balances, ref IRiskResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}