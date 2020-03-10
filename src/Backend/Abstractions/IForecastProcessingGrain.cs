using System.Threading.Tasks;
using Contracts.Enums;
using Contracts.Trading;
using Orleans;

namespace Abstractions
{
	public interface IForecastProcessingGrain : IGrainWithIntegerCompoundKey
	{
		Task<TradingAdvices> GetForecast(ITradingContext tradingContext);
		
		Task<TradingAdvices> GetAllForecast(ITradingContext tradingContext);
	}
}