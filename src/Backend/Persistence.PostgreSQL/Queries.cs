namespace Persistence
{
	static class Queries
	{
		private static string CreateSchema = @"CREATE SCHEMA IF NOT EXISTS @name;";
		
		private static string DropSchema = @"CREATE SCHEMA IF EXISTS @name;";

		// Create trades table
		
		static string GetCandles = @"
                with intervals as 
				(
					select 
						start,
						start + interval @period as end
					from 
						generate_series(@from, @to, interval @period) as start
				)
				select distinct
					intervals.start as date,
					min(price) over w as low,
					max(price) over w as high,
					first_value(price) over w as open,
					last_value(price) over w as close,
					sum(quantity) over w as volume
				from
					intervals
				join 
					{exchange}.{asset1}_{asset2}_trades tr on
					tr.time >= intervals.start 
				and
					tr.time < intervals.end
				window w as (partition by intervals.start order by tr.date asc rows between unbounded preceding and unbounded following)
				order by 
					intervals.start;";
	}
}
