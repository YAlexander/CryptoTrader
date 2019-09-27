using System.Data;
using System.Threading.Tasks;
using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using Dapper;

namespace core.Infrastructure.Database.Managers
{
	public class BookManager : IBookManager
	{
		public Task<long> Create (Book entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"
				insert into
					Book
					(
						id,
						created,
						isDeleted,
						exchangeCode,
						symbol,
						bestAskPrice,
						bestAskQuantity,
						bestBidPrice,
						bestBidQuantity
					)
				values
					(
						default,
						@created,
						false,
						@exchangeCode,
						@symbol,
						@bestAskPrice,
						@bestAskQuantity,
						@bestBidPrice,
						@bestBidQuantity
					)
				returning id;
				";

			return connection.QuerySingleAsync<long>(query, new
			{
				created = entity.Created,
				exchangeCode = entity.ExchangeCode,
				symbol = entity.Symbol,
				bestAskPrice = entity.BestAskPrice,
				bestAskQuantity = entity.BestAskQuantity,
				bestBidPrice = entity.BestBidPrice,
				bestBidQuantity = entity.BestBidQuantity
			}, transaction);
		}

		public Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}

		public Task<Book> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}

		public Task<Book> GetLast (int exchangeCode, string symbol, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Book where exchangeCode = @exchangeCode and symbol = @symbol order by created desc limit 1;";
			return connection.QuerySingleOrDefaultAsync<Book>(query, new { exchangeCode = exchangeCode, symbol = symbol }, transaction);
		}

		public Task<Book> Update (Book entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new System.NotImplementedException();
		}
	}
}
