using System.Threading.Tasks;
using Abstractions;
using Common;
using Contracts.Trading;
using Orleans;

namespace Core.OrleansInfrastructure.Grains
{
    // TODO: Add State and storage
    public class StrategyGrain<StrategyInfo> : Grain, IStrategyGrain
    {
        public async Task<IStrategyInfo> Get()
        {
            long primaryKey = this.GetPrimaryKeyLong(out string keyExtension);
            GrainKeyExtension secondaryKey = keyExtension.ToExtended();
            
            return new Persistence.Entities.StrategyInfo();
        }
    }
}