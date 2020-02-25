using System.Threading.Tasks;
using Contracts;
using Contracts.Trading;
using Persistence;

namespace Core.BusinessLogic.TradingConstraints
{
    public class MaxAmountConstraint : ITradingConstraint<decimal>
    {
        private readonly ITradesManager _tradesManager;
        public MaxAmountConstraint (ITradesManager tradesManager)
        {
            _tradesManager = tradesManager;
        }
        
        public async Task<decimal> Get(ICandle[] candles, IStrategyInfo info, decimal[] balances)
        {
            // Fmax = 1 / g * (P / B - (1 - P) / A)
            // g - leverage
            // P = lim M / N where M - number of profitable deals, N - total deals
            // B - loss when StopLoss is triggered
            // A - profit when TakeProfit is triggered
            // Optimal F is from 0.3 to 0.5 of Fmax
            // x = Fmax / K
            // x - deal amount
            // K - total funds

            int totalTrades = await _tradesManager.GetNumberOfProfitableDeals(info.Exchange, info.Asset1Code, info.Asset2Code);

            
            
            throw new System.NotImplementedException();
        }
    }
}