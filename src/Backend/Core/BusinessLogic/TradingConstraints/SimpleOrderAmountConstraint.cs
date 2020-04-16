using System;
using System.Linq;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Enums;

namespace Core.BusinessLogic.TradingConstraints
{
    public class SimpleOrderAmountConstraint : IRiskManager
    {
        public string Name { get; set; } = "Max Order Amount (Simple)";

        public ITradingContext Process(ITradingContext context, IStrategyInfo info)
        {
            IBalance funds = context.Deal.Position == DealPositions.LONG 
                ? context.Funds.FirstOrDefault(x => x.Asset == context.TradingPair.asset2)
                : context.Funds.FirstOrDefault(x => x.Asset == context.TradingPair.asset1);

            if (funds == null) throw new Exception($"Insufficient funds");
            
            int leverage = info.UseMarginalTrading ? info.Leverage : 1;
            context.MaxAmount = (funds.TotalAmount - funds.LockedAmount) * leverage * 0.01m;
            return context;
        }
    }
}