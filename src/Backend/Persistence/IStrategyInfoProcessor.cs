using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence
{
	public interface IStrategyInfoProcessor
	{
		Task<StrategyInfo> GetStrategyInfo(Exchanges exchange, Assets asset1, Assets asset2);
	}
}