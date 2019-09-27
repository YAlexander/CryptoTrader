using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace core.Infrastructure.Database.Managers
{
	public class TradesManager : ITradesManager
	{
		public Task<int> ClearData (int exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"
					delete from 
							Trades
					where
						exchangecode = @exchangecode
					and
						symbol = @symbol
					and
						created != (select max(created)
					from 
						Trades)
					";

			return connection.ExecuteAsync(query, new { exchangeCode = exchangeCode, symbol = symbol }, transaction);
		}

		public Task<long> Create (Trade entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"
				insert into
					Trades
					(
						id,
						created,
						time,
						exchangeCode,
						symbol,
						tradeId,
						quantity,
						price,
						isDeleted
					)
				values
					(
						default,
						@created,
						@time,
						@exchangeCode,
						@symbol,
						@tradeId,
						@quantity,
						@price,
						false
					)
				returning id;";

			return connection.QuerySingleAsync<long>(query,
				new
				{
					created = entity.Created,
					time = entity.Time,
					exchangeCode = entity.ExchangeCode,
					symbol = entity.Symbol,
					tradeId = entity.TradeId,
					quantity = entity.Quantity,
					price = entity.Price
				}, transaction);
		}

		public Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"update Trades set isDeleted = true where id = @id";
			return connection.ExecuteAsync(query, new { id = id }, transaction);
		}

		public Task<Trade> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Trades where id = @id;";
			return connection.QueryFirstOrDefaultAsync<Trade>(query, new { id = id }, transaction);
		}

		public Task<Trade> GetLast (int exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Trades where exchangeCode = @exchangeCode and symbol = @symbol order by created limit 1;";
			return connection.QueryFirstOrDefaultAsync<Trade>(query,
				new
				{
					exchangeCode = exchangeCode,
					symbol = symbol
				}, transaction);
		}

		public Task<Trade> Update (Trade entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			// Trade should not be updated
			throw new NotImplementedException();
		}
	}
}
