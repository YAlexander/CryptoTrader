using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Contracts.Enums;
using Dapper;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class ExchangeSettingsManager : IExchangeSettingsManager
	{
		public Task<IEnumerable<IExchangeSettings>> Get(Exchanges exchange, IDbConnection connection, IDbTransaction transaction = null)
		{
			return connection.QueryAsync<IExchangeSettings>(GetSettings, new { exchange = exchange }, transaction);
		}

		private const string GetSettings = @"
				select
				     *
				from 
				     ExchangeSettings
				where
				      exchange = @exchange;";
	}
}