using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using core.Abstractions.Database;
using core.Abstractions.TypeCodes;
using core.Infrastructure.Database.Entities;
using Dapper;

namespace core.Infrastructure.Database.Managers
{
	public class CandleManager : ICandleManager
	{
		public async Task<long> Create (Candle entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"
					insert into
						Candles
						(
							id,
							created,
							symbol,
							exchangeCode,
							time,
							periodCode,
							openTime,
							closeTime,
							open,
							close,
							high,
							low,
							volume,
							numberOfTrades,
							isDeleted
						)
					select
							nextval('candles_id_seq'),
							@created,
							@symbol,
							@exchangeCode,
							@time,
							@periodCode,
							@openTime,
							@closeTime,
							@open,
							@close,
							@high,
							@low,
							@volume,
							@numberOfTrades,
							false
					where not exists 
						(select 1 from Candles where symbol = @symbolCode and exchangeCode = @exchange and openTime = @openTime and closeTime = @closeTime)
					returning
						id;
			";

			return await connection.ExecuteAsync(query, new
			{
				created = entity.Created,
				symbol = entity.Symbol.ToUpper(),
				symbolCode = entity.Symbol.ToUpper(),
				exchangeCode = entity.ExchangeCode,
				exchange = entity.ExchangeCode,
				time = entity.Time,
				periodCode = entity.PeriodCode,
				openTime = entity.OpenTime,
				closeTime = entity.CloseTime,
				open = entity.Open,
				close = entity.Close,
				high = entity.High,
				low = entity.Low,
				volume = entity.Volume,
				numberOfTrades = entity.NumberOfTrades
			}, transaction);
		}

		public async Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"update Candles set isDeleted = true where id = @id";
			await connection.ExecuteAsync(query, new { id = id }, transaction);
		}

		public async Task<Candle> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select * from Candles where id = @id";
			return await connection.ExecuteScalarAsync<Candle>(query, new { id = id }, transaction);
		}

		public Task<IEnumerable<Candle>> GetLastCandles (int exchangeCode, string symbol, int interval, int numberOfCandles, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select
								*
							from
								Candles
							where
								exchangeCode = @exchangeCode
							and
								symbol = @symbol
							and
								periodCode = @interval
							order by
								time desc 
							limit
								@numberOfCandles";

			return connection.QueryAsync<Candle>(query, 
					new 
					{ 
						exchangeCode  = exchangeCode,
						symbol = symbol,
						interval = interval,
						numberOfCandles = @numberOfCandles }, transaction);
		}

		public Task<Candle> Update (Candle entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			// Candle can't be changed!
			throw new System.NotImplementedException();
		}
	}
}
