using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;

namespace core.Infrastructure.Database.Managers
{
	public class ExchangeConfigManager : IExchangeConfigManager
	{
		public async Task<long> Create (ExchangeConfig entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"
					insert into 
						ExchangeConfigs
						(
							id,
							created,
							updated,
							isEnabled,
							isDeleted,
							exchangeCode,
							apiKey,
							apiSecret,
						)
					values
						(
							default,
							@created,
							null,
							true,
							false,
							@exchangeCode,
							@key,
							@secret
						)
					returning id
					on conflict (exchangeCode) do nothing;";

			return await connection.QuerySingleAsync<long>(query,
				new
				{
					created = DateTime.UtcNow,
					exchangeCode = entity.ExchangeCode,
					key = entity.ApiKey,
					secret = entity.ApiSecret
				}, transaction);
		}

		public async Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"update ExchangeConfigs set isDeleted = true where exchangeCode = @exchangeCode;";
			await connection.ExecuteAsync(query, new { id = id }, transaction);
		}

		public async Task<ExchangeConfig> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select * from ExchangeConfigs where exchangeCode = @exchangeCode;";
			return await connection.QueryFirstOrDefaultAsync<ExchangeConfig>(query, new { id = id }, transaction);
		}

		public async Task<IEnumerable<ExchangeConfig>> GetAll (IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select * from ExchangeConfigs;";
			return await connection.QueryAsync<ExchangeConfig>(query, new { }, transaction);
		}

		public async Task<ExchangeConfig> Update (ExchangeConfig entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"update
								ExchangeConfig
							set
								updated = @updated,
								isEnabled = @enabled,
								apiKey = @apiKey,
								apiSecret = @apiSecret
							where
								exchangeCode = @exchangeCode;
						";
			return await connection.QueryFirstOrDefaultAsync<ExchangeConfig>(query,
				new
				{
					updated = DateTime.UtcNow,
					isEnabled = entity.IsEnabled,
					apiKey = entity.ApiKey,
					apiSecret = entity.ApiSecret
				}, transaction);
		}
	}
}
