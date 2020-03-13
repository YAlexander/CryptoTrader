using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence
{
	public interface ISettingsProcessor
	{
		public Task<IEnumerable<IExchangeSettings>> Get(Exchanges exchange);
	}
}