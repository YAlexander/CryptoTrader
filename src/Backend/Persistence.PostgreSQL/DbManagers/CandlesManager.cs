using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Contracts.Enums;
using Dapper;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class CandlesManager : ICandlesManager
	{
		public Task<IEnumerable<Candle>> Get(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles, IDbConnection connection, IDbTransaction transaction = null)
		{
			return connection.QueryAsync<Candle>(string.Format(GetCandles, exchange), new
			{
				asset1 = asset1,
				asset2 = asset2,
				numberOfCandles = numberOfLastCandles
			}, transaction);
		}

		public Task<Candle> Get(Exchanges exchange, Assets asset1, Assets asset2, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}

		public Task<Candle> Create(Candle candle, IDbConnection connection, IDbTransaction transaction = null)
		{
			return connection.QueryFirstAsync<Candle>(string.Format(CreateCandle, candle.Exchange, candle.Asset1, candle.Asset2),
				new
						{
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

		private const string GetCandles = @"
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
		
		private const string CreateCandle = @"
				insert into {0}.Candles 
									(
										created,
										updated,
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
										@updated,
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
												{0}.Candles 
											where 
												asset1 = @asset1
											and
											 	asset2 = @asset2
											and
											 	time = @time  										
										)
									returning *;
		";
		
		#endregion
	}
}