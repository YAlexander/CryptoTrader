﻿using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Abstractions.Enums;
using Dapper;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class ExchangeSettingsManager : IExchangeSettingsManager
	{
		public Task<IEnumerable<IExchangeSettings>> Get(Exchanges exchange, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = $@"select * from {exchange}.ExchangeSettings";
			return connection.QueryAsync<IExchangeSettings>(query, new { }, transaction);
		}
	}
}