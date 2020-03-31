using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Abstractions.Enums;
using Dapper;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class CandlesManager : ICandlesManager
	{
		public Task<IEnumerable<Candle>> Get(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles, IDbConnection connection, IDbTransaction transaction = null)
		{
			return connection.QueryAsync<Candle>(string.Format(GetLastCandles, exchange), new
			{
				asset1 = asset1,
				asset2 = asset2,
				numberOfCandles = numberOfLastCandles
			}, transaction);
		}

		public Task<Candle> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime time, IDbConnection connection, IDbTransaction transaction = null)
		{
			return connection.QueryFirstAsync<Candle>(string.Format(GetLastCandles, exchange), new
			{
				asset1 = asset1,
				asset2 = asset2,
				time = time
			}, transaction);
		}

		public Task<IEnumerable<Candle>> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to, IDbConnection connection, IDbTransaction transaction = null)
		{
			return connection.QueryAsync<Candle>(string.Format(GetLastCandles, exchange), new
			{
				asset1 = asset1,
				asset2 = asset2,
				from = from,
				to = to
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

		#region queries

		private const string GetLastCandles = @"
				select 
					*
				from
					{0}.Candles
				where 
					asset1 = @asset1
				and
					asset2 = @asset2
				order by time desc
				limit @numberOfCandles;
		";
		
		private const string GetCandle = @"
				select 
					*
				from
					{0}.Candles
				where 
					asset1 = @asset1
				and
					asset2 = @asset2
				and 
					time = @time
		";
		
		private const string GetCandlesByDates = @"
				select 
					*
				from
					{0}.Candles
				where 
					asset1 = @asset1
				and
					asset2 = @asset2
				and 
					time between @from and @to
		";
		#endregion
	}
}