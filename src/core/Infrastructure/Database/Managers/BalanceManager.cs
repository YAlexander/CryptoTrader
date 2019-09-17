using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using System;
using System.Data;
using Dapper;
using System.Threading.Tasks;

namespace core.Infrastructure.Database.Managers
{
	public class BalanceManager : IBalanceManager
	{
		public Task<long> Create (Balance entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"
						insert into
							Balances
							(
								id,
								created,
								updated,
								isDeleted,
								exchangeCode,
								asset,
								total,
								available
							)
						values
							(
								default,
								@created,
								null,
								false,
								@exchangeCode,
								@asset,
								@total,
								@available
							);
						returning id;
					";

			return connection.QuerySingleAsync<long>(query,
				new 
				{
					created = entity.Created,
					exchangeCode = entity.ExchangeCode,
					asset = entity.Asset,
					total = entity.Total,
					available = entity.Available
				}, transaction);
		}

		public Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}

		public Task<Balance> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Balances where id = @id;";
			return connection.QueryFirstOrDefaultAsync<Balance>(query, new { id = id }, transaction);
		}

		public Task<Balance> Get (string asset, long exchangeCode, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Balances where asset = @asset and exchangeCode = @exchangeCode;";
			return connection.QueryFirstOrDefaultAsync<Balance>(query, new { asset = asset, exchangeCode  = exchangeCode }, transaction);
		}

		public Task<Balance> Update (Balance entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"
						update
							Balances
						set
							updated = @updated,
							total = @total,
							available = @available
						where
							id = @id;
					";

			return connection.QueryFirstOrDefaultAsync<Balance>(query,
				new
				{
					updated = entity.Updated,
					total = entity.Total,
					available = entity.Available
				}, transaction);
		}
	}
}
