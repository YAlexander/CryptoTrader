using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace core.Infrastructure.Database.Managers
{
	public class DealManager : IDealManager
	{
		public Task<long> Create (Deal entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"
				insert into
					Deals
					(
						id,
						created,
						updated,
						statusCode,
						isEnabled,
						isDeleted,
						exchangeCode,
						symbol,
						avgOpenPrice,
						avgClosePrice,
						amount,
						estimatedFee
					)
				values
					(
						default,
						@created,
						null,
						@statusCode,
						true,
						false,
						@exchangeCode,
						@symbol,
						@avgOpenPrice,
						@avgClosePrice,
						@amount,
						@estimatedFee
					)
				returning id;
			";

			return connection.QuerySingleAsync<long>(query,
				new
				{
					created = entity.Created,
					statusCode = entity.StatusCode,
					exchangeCode = entity.Exchange,
					symbol = entity.Symbol,
					avgOpenPrice = entity.AvgOpenPrice,
					avgClosePrice = entity.AvgClosePrice,
					amount = entity.Amount,
					estimatedFee = entity.EstimatedFee
				}, transaction);
		}

		public Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = "update Deals set isDeleted where id = @id;";
			return connection.ExecuteAsync(query, new { id = id }, transaction);
		}

		public Task<Deal> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select * from Deals where id = @id";
			return connection.QuerySingleOrDefaultAsync<Deal>(query, new { id = id }, transaction);
		}

		public Task<IEnumerable<Order>> GetAssociatedOrders (long dealId, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Orders where dealId = @dealId";
			return connection.QueryAsync<Order>(query, new { dealId = dealId }, transaction);
		}

		public Task<IEnumerable<Deal>> GetOpenDeals (int exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Deals where status = 0 and exchangeCode = @exchangeCode and symbol = @symbol;";
			return connection.QueryAsync<Deal>(query, new { exchangeCode = exchangeCode, symbol = symbol }, transaction);
		}

		public Task<Deal> Update (Deal entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"
					update
						Deals
					set
						statusCode = @statusCode,
						avgOpenPrice = @avgOpenPrice,
						avgClosePrice = @avgClosePrice,
						amount = @amount,
						estimatedFee = @estimatedFee
					where
						id = @id;
					";

			return connection.QuerySingleOrDefaultAsync<Deal>(query,
				new
				{
					statusCode = entity.StatusCode,
					avgOpenPrice = entity.AvgOpenPrice,
					avgClosePrice = entity.AvgClosePrice,
					amount = entity.Amount,
					estimatedFee = entity.EstimatedFee,
					id = entity.Id
				}, transaction);
		}
	}
}