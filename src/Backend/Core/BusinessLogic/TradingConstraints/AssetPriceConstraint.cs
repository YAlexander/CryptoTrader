using System.Threading.Tasks;
using Abstractions;

namespace Core.BusinessLogic.TradingConstraints
{
    public class AssetPriceConstraint : ITradingConstraint
    {
        public async Task<ITradingContext> Set (ITradingContext context)
        {
            return context;
        }
    }
}