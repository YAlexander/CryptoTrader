using System.Threading.Tasks;
using Abstractions;
using Abstractions.Entities;

namespace Core.BusinessLogic.TradingConstraints
{
    public class AssetPriceConstraint : IRiskManager
    {
        public ITradingContext Process(ITradingContext context, IStrategyInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}