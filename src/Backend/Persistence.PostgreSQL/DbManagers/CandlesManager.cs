using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Contracts;
using Contracts.Enums;
using Persistence.Entities;
using Persistence.Managers;

namespace Persistence.PostgreSQL.DbManagers
{
	public class CandlesManager : ICandlesManager
	{
		public Task<IEnumerable<ICandle>> Get(Exchanges exchange, Assets asset1, Assets asset2, int numberOfLastCandles, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}

		public Task<Candle> Get(Exchanges exchange, Assets asset1, Assets asset2, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}

		public Task<Candle> Create(Candle obj, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}

		public Task<Candle> Update(Candle obj, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}

		#region queries
		
		

		#endregion

		/*
		public Task<IEnumerable<Candle>> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to,  Timeframes period, IDbConnection connection, IDbTransaction transaction)
		{
			return connection.QueryAsync<Candle>(BuildCandlestick, new
			{
				exchange = exchange,
				asset1 = asset1,
				asset2 = asset2,
				startDate = from,
				endDate = to,
				period = GetPeriod(period)
			}, transaction);
		}

		private string GetPeriod(Timeframes period)
		{
			return period switch
			{
				Timeframes.MINUTE => "1min",
				Timeframes.FIVE_MINUTES => "5min",
				Timeframes.QUARTER_HOUR => "15min",
				Timeframes.HALF_HOUR => "30min",
				Timeframes.HOUR => "1hour",
				Timeframes.FOUR_HOUR => "4hours",
				Timeframes.DAY => "1day",
				Timeframes.WEEK => "1week",
				_ => throw new Exception("Unknown interval")
			};
		}
		
		private const string BuildCandlestick = @"
				with intervals as (
					select start,
					start + interval @period as end
				from
					generate_series(@startDate, @endDate', interval @period) as start
				)
				select distinct
					intervals.start as date,
					min(price) over w as low,
					max(price) over w as high,
					first_value(price) over w as open,
					last_value(price) over w as close,
					sum(quantity) over w as volume
				from
					trades trd
				where
					trd.exchange = @exchange
				and
					trd.asset1 = @asset1
				and
					trd.asset2 = @asset2
				and
					trd.date >= intervals.start
				and
					trd.date < intervals.end
				window w as (
						partition by intervals.start 
						order by trd.date asc 
						rows between unbounded preceding and unbounded following)
				order by intervals.start
		";
	*/
	}
}