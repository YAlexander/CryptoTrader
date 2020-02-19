using System.Threading.Tasks;
using Contracts.Enums;
using Contracts.Trading;

namespace Persistence.StrategyOptions.OptionManagers
{
	public interface IStrategyOptionsManager<T> where T : IStrategyOption 
	{
		string StrategyName { get; }
		
		Task<T> GetOptions(Exchanges exchange, Assets asset1, Assets asset2, string strategyName);
	}
}