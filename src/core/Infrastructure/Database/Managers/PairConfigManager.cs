using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;

namespace core.Infrastructure.Database.Managers
{
	public class PairConfigManager : IPairConfigManager
	{
		public async Task<long> Create (PairConfig entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"insert into
								PairConfigs
								(
									id,
									created,
									updated,
									isEnabled,
									isDeleted,
									exchangeCode,
									symbol,
									strategyId,
									refreshDelaySeconds,
									defaultStopLossPercent,
									defaultTakeProfitPercent,
									isTestMode,
									exchangeFeeSell,
									exchangeFeeBuy,
									tradingLockedTill,
									isMaxAmountPercent,
									maxOrderAmount
								)
							values
								(
									default,
									@created,
									null,
									true,
									false,
									@exchangeCode,
									@symbol,
									@strategyId,
									@refreshDelaySeconds,
									@defaultStopLossPercent,
									@defaultTakeProfitPercent,
									@isTestMode,
									@exchangeFeeSell,
									@exchangeFeeBuy,
									@tradingLockedTill,
									@isMaxAmountPercent,
									@maxOrderAmount
								)
							returning id;";

			return await connection.QuerySingleAsync<long>(query,
				new
				{
					created = DateTime.UtcNow,
					exchangeCode = entity.ExchangeCode,
					symbol = entity.Symbol,
					strategyId = entity.StrategyId,
					refreshDelaySeconds = entity.RefreshDelaySeconds,
					defaultStopLossPercent = entity.DefaultStopLossPercent,
					defaultTakeProfitPercent = entity.DefaultTakeProfitPercent,
					isTestMode = entity.IsTestMode,
					exchangeFeeSell = entity.ExchangeFeeSell,
					exchangeFeeBuy = entity.ExchangeFeeBuy,
					tradingLockedTill = entity.TradingLockedTill,
					isMaxAmountPercent = entity.isMaxAmountPercent,
					maxOrderAmount = entity.MaxOrderAmount
				}, transaction);
		}

		public async Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"update PairConfigs set isDeleted = true where id = id;";
			await connection.ExecuteAsync(query, new { id = id }, transaction);
		}

		public async Task<PairConfig> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select * from PairConfigs where id = @id;";
			return await connection.QueryFirstOrDefaultAsync<PairConfig>(query, new { id = id });
		}

		public async Task<PairConfig> Update (PairConfig entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"update
								PairConfigs
							set
								updated = @updated,
								isEnabled = @isEnabled,
								strategyId = @strategyId,
								refreshDelaySeconds = @refreshDelaySeconds,
								defaultStopLossPercent = @defaultStopLossPercent,
								defaultTakeProfitPercent = @defaultTakeProfitPercent,
								isTestMode = @isTestMode,
								exchangeFeeSell = @exchangeFeeSell,
								exchangeFeeBuy = @exchangeFeeBuy,
								tradingLockedTill = @tradingLockedTill,
								isMaxAmountPercent = @isMaxAmountPercent,
								maxOrderAmount = @maxOrderAmount
							where
								id = @id
							returning *;";

			return await connection.QuerySingleAsync<PairConfig>(query,
				new
				{
					updated = DateTime.UtcNow,
					isEnabled = entity.IsEnabled,
					strategyId = entity.StrategyId,
					refreshDelaySeconds = entity.RefreshDelaySeconds,
					defaultStopLossPercent = entity.DefaultStopLossPercent,
					defaultTakeProfitPercent = entity.DefaultTakeProfitPercent,
					isTestMode = entity.IsTestMode,
					exchangeFeeSell = entity.ExchangeFeeSell,
					exchangeFeeBuy = entity.ExchangeFeeBuy,
					tradingLockedTill = entity.TradingLockedTill,
					isMaxAmountPercent = entity.isMaxAmountPercent,
					maxOrderAmount = entity.MaxOrderAmount
				}, transaction);
		}

		public async Task<IEnumerable<PairConfig>> GetAssignedPairs (long exchangeCode, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select 
								pc.*,
								s.optimalTimeframe as timeframe
							from 
								PairConfigs pc 
							left join 
								Strategies s on pc.strategyId = s.id
							where 
								exchangeCode = @exchangeCode;";

			return await connection.QueryAsync<PairConfig>(query, new { exchangeCode = exchangeCode }, transaction);
		}
	}
}
