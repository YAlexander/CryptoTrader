using core.Infrastructure.Database.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IExchangeConfigManager : IDatabaseManager<ExchangeConfig>
	{
		Task<IEnumerable<ExchangeConfig>> GetAll (IDbConnection connection, IDbTransaction transaction = null);
	}
}
