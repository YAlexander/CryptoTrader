using System;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using System.Collections.Generic;
using core.Abstractions.TypeCodes;

namespace core.Infrastructure.Database.Managers
{
	public class StrategyManager : IStrategyManager
	{
		public Task<long> Create (Strategy entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}

		public Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> Enable (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"update Strategies set isEnabled = not isEnabled where id = @id";
			return await connection.ExecuteAsync(query, new { id = id }, transaction) > 0;
		}

		public Task<Strategy> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select * from Strategies where id = @id";
			return connection.QueryFirstAsync<Strategy>(query, new { id = id }, transaction);
		}

		public Task<Strategy> Get (IExchangeCode exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select
								*
							from
								Strategies str
							join
								PairConfigs pcfg
							on
								str.id = pcfg.strategyId
							where
								str.isDeleted = false
							and
								pcfg.exchangeCode = @exchangeCode
							and
								pcfg.symbol = @symbol;";

			return connection.QueryFirstOrDefaultAsync<Strategy>(query, 
				new 
				{
					exchangeCode = exchangeCode.Code,
					symbol = symbol
				}, transaction);
		}

		public Task<IEnumerable<Strategy>> GetAll (IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select * from Strategies";
			return connection.QueryAsync<Strategy>(query, transaction);
		}

		public Task<Strategy> Update (Strategy entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}
	}
}
