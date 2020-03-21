using System.Threading.Tasks;
using Abstractions;
using Common.Trading;

namespace Core.BusinessLogic.TradingConstraints
{
    public class TakeProfitConstraint : ITradingConstraint
    {
        public async Task<ITradingContext> Set(ITradingContext context)
        {
            return context;
        }
    }
}