using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence
{
	public interface ISettingsProcessor
	{
		Task<IEnumerable<IExchangeSettings>> Get(Exchanges exchange);
		Task<IExchangeSettings> Get(Exchanges exchange, Assets asset1, Assets asset2);
	}
}