using System.Threading.Tasks;
using Contracts.Trading;

namespace Core.BusinessLogic.TradingConstraints
{
    public class DealCommissionConstraint : ITradingConstraint
    {
        public async Task<ITradingContext> Set(ITradingContext context)
        {
            return context;
        }
    }
}