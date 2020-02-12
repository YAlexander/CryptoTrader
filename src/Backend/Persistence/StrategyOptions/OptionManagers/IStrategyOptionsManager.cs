using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence.StrategyOptions.OptionManagers
{
	public interface IStrategyOptionsManager<T>
	{
		Task<IEnumerable<T>> GetOptions(Exchanges exchange, Assets asset1, Assets asset2, string strategyName);
	}
}