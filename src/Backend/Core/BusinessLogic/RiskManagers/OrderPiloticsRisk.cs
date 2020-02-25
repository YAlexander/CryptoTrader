using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Contracts.Trading;

namespace Core.BusinessLogic.RiskManagers
{
    public class OrderPoliticsRisk : IRisk<FillPolitics>
    {
        public async Task<FillPolitics> Get(ICandle[] candles, IStrategyInfo info, decimal[] balances)
        {
            throw new System.NotImplementedException();
        }
    }
}