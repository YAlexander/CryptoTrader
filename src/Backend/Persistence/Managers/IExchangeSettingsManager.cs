using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Abstractions.Enums;
using Persistence.Entities;

namespace Persistence.Managers
{
	public interface IExchangeSettingsManager
	{
		Task<IEnumerable<IExchangeSettings>> Get(Exchanges exchange, IDbConnection connection, IDbTransaction transaction = null);
	}
}