using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Enums;
using Persistence.Entities;

namespace Persistence
{
	public interface ISettingsProcessor
	{
		Task<IEnumerable<ExchangeSettings>> Get(Exchanges exchange);
		Task<ExchangeSettings> Get(Exchanges exchange, Assets asset1, Assets asset2);
	}
}