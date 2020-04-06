using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Abstractions.Enums;
using Dapper;
using Persistence.Entities;
using Persistence.Managers;
using TechanCore;

namespace Persistence.PostgreSQL.DbManagers
{
	public class CandlesManager : ICandlesManager
	{
		public Task<IEnumerable<Candle>> Get(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = $@"
				select 
					*
				from
					{exchange}.Candles
				where 
					asset1 = @asset1
				and
					asset2 = @asset2
				order by time desc
				limit @numberOfCandles;
			";

			return connection.QueryAsync<Candle>(string.Format(query, exchange), new
			{
				asset1,
				asset2,
				numberOfCandles = numberOfLastCandles
			}, transaction);
		}

		public Task<Candle> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime time, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = $@"
				select 
					*
				from
					{exchange}.Candles
				where 
					asset1 = @asset1
				and
					asset2 = @asset2
				and
					time = @time
				order by time desc
				limit @numberOfCandles;
			";

			return connection.QueryFirstAsync<Candle>(query, new
			{
				asset1,
				asset2,
				time
			}, transaction);
		}

		public Task<IEnumerable<Candle>> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime @from, DateTime to, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = $@"
				select 
					*
				from
					{exchange}.Candles
				where
 					asset1 = @asset1
				and
					asset2 = @asset2
				and
					time between @from and @to
				order by time desc
				limit @numberOfCandles;
			";

			return connection.QueryAsync<Candle>(query, new
			{
				asset1,
				asset2,
				from,
				to
			}, transaction);
		}

		public Task<Candle> Create(Candle candle, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = $@"
				insert into {candle.Exchange}.Candles 
					(
						created,
						exchange,
						asset1,
						asset2,
						time,
						timeFrame,
						high,
						low,
						open,
						close,
						volume,
						trades
					)
					select									
						@created,
						@exchange,
						@asset1,
						@asset2,
						@time,
						@timeFrame,
						@high,
						@low,
						@open,
						@close,
						@volume,
						@trades
					where 
						not exists 
						(
							select 
								1 
							from 
								{candle.Exchange}.Candles 
							where 
								asset1 = @asset1
							and
								asset2 = @asset2
							and
								time = @time  										
						)
						returning *;
				";

			return connection.QueryFirstAsync<Candle>(query,
				new
						{
							created = DateTime.Now,
							exchange = (int)candle.Exchange,
							asset1 = (int)candle.Asset1,
							asset2 = (int)candle.Asset2,
							time = candle.Time,
							timeFrame = (int)candle.TimeFrame,
							high = candle.High,
							low = candle.Low,
							open = candle.Open,
							close = candle.Close,
							volume = candle.Volume,
							trades = candle.Trades
						}, transaction);
		}

		public Task<Candle> Update(Candle obj, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}
	}
}