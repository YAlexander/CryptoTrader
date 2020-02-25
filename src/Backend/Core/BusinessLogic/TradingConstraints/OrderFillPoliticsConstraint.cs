using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Contracts.Trading;

namespace Core.BusinessLogic.TradingConstraints
{
    public class OrderFillPoliticsConstraint : ITradingConstraint<FillPolitics>
    {
        public async Task<FillPolitics> Get(ICandle[] candles, IStrategyInfo info, decimal[] balances)
        {
            throw new System.NotImplementedException();
        }
    }
}