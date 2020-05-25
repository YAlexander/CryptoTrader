using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Enums;
using Dapper;
using Orleans.Hosting;
using Persistence.Entities;
using Persistence.Managers;
using TechanCore;

namespace Persistence.PostgreSQL.DbManagers
{
	public class CandlesManager : ICandlesManager
	{
		public Task<IEnumerable<Entities.CandleEntity>> Get(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles, IDbConnection connection, IDbTransaction transaction = null)
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

			return connection.QueryAsync<Entities.CandleEntity>(string.Format(query, exchange), new
			{
				asset1,
				asset2,
				numberOfCandles = numberOfLastCandles
			}, transaction);
		}

		public Task<Entities.CandleEntity> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime? time, IDbConnection connection, IDbTransaction transaction = null)
		{
			string timeCondition = time.HasValue ? " and time = @time " : String.Empty;
			string query = $@"
				select 
					*
				from
					{exchange}.Candles
				where 
					asset1 = @asset1
				and
					asset2 = @asset2
				{time}
				order by time desc limit 1;
			";

			return connection.QueryFirstAsync<Entities.CandleEntity>(query, new
			{
				asset1,
				asset2,
				time
			}, transaction);
		}

		public Task<IEnumerable<Entities.CandleEntity>> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime @from, DateTime to, IDbConnection connection, IDbTransaction transaction = null)
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

			return connection.QueryAsync<Entities.CandleEntity>(query, new
			{
				asset1,
				asset2,
				from,
				to
			}, transaction);
		}

		public Task<Entities.CandleEntity> Create(Entities.CandleEntity candle, IDbConnection connection, IDbTransaction transaction = null)
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

			return connection.QueryFirstAsync<Entities.CandleEntity>(query,
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

		public Task<Entities.CandleEntity> Update(Entities.CandleEntity obj, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}
	}
}