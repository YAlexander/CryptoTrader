using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using core.Infrastructure.Models;
using System.Collections.Generic;

namespace core.Infrastructure.Database.Managers
{
	public class OrdersManager : IOrderManager
	{
		public Task<long> Create (Order entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"
				insert into
					Orders
					(
						id,
						created,
						updated,
						isEnabled,
						isDeleted,
						exchangeCode,
						orderSideCode,
						orderTypeCode,
						orderStatusCode,
						fillPoliticsCode,
						tradingModeCode,
						dealId,
						exchangeOrderId,
						exchangeOrderStatusCode,
						symbol,
						price,
						amount,
						limitPrice,
						stopLimitPrice,
						stopLoss,
						takeProfit,
						expirationDate,
						lastErrorDate,
						lastError,
						statusDescription,
						isUpdateRequired,
						isCancelRequired
					)
					values
					(
						default,
						@created,
						null,
						true,
						false,
						@exchangeCode,
						@orderSideCode,
						@orderTypeCode,
						@orderStatusCode,
						@fillPoliticsCode,
						@tradingModeCode,
						@dealId,
						@exchangeOrderId,
						@exchangeOrderStatusCode,
						@symbol,
						@price,
						@amount,
						@limitPrice,
						@stopLimitPrice,
						@stopLoss,
						@takeProfit,
						@expirationDate,
						@lastErrorDate,
						@lastError,
						@statusDescription,
						false,
						false
					)
					returning
						id;";

			return connection.QuerySingleAsync<long>(query,
				new
				{
					created = entity.Created,
					exchangeCode = entity.ExchangeCode,
					orderSideCode = entity.OrderSideCode,
					orderTypeCode = entity.OrderTypeCode,
					orderStatusCode = entity.OrderStatusCode,
					fillPoliticsCode = entity.FillPoliticsCode,
					dealId = entity.DealId,
					exchangeOrderId = entity.ExchangeOrderId,
					exchangeOrderStatusCode = entity.ExchangeOrderStatusCode,
					symbol = entity.Symbol,
					price = entity.Price,
					amount = entity.Amount,
					limitPrice = entity.Limit,
					stopLimitPrice = entity.StopLimit,
					stopLoss = entity.StopLoss,
					takeProfit = entity.TakeProfit,
					expirationDate = entity.ExpirationDate,
					LastErrorDate = entity.LastErrorDate,
					lastError = entity.LastError,
					tradingModeCode = entity.TradingModeCode,
					statusDescription = entity.StatusDescription
				}, transaction);
		}

		public Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = "update Orders set isDeleted where id = @id";
			connection.ExecuteAsync(query, new { id = id }, transaction);
			return Task.CompletedTask;
		}

		public Task<Order> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"select * from Orders where id = @id;";
			return connection.QueryFirstOrDefaultAsync<Order>(query, new { id = id }, transaction);
		}

		public Task<IEnumerable<Order>> GetOrdersByDealId (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Orders where dealId = @dealId;";
			return connection.QueryAsync<Order>(query, new { dealId = id }, transaction);
		}

		public Task<Order> Update (Order entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			string query = @"
					update
						Orders
					set
						orderStatusCode = @orderStatusCode,
						updated = @updated,
						exchangeOrderId = @exchangeOrderId,
						exchangeOrderStatusCode = @exchangeOrderStatusCode,
						lastErrorDate = @lastErrorDate,
						lastError = @lastError,
						updateRequired = @updateRequired,
						cancelRequired = @cancelRequired,
						statusDescription = @statusDescription
					where
						id = @id
					returning *;
					";

			return connection.QueryFirstAsync<Order>(query,
				new
				{
					orderStatusCode = entity.OrderStatusCode,
					updated = entity.Updated,
					exchangeOrderId = entity.ExchangeOrderId,
					exchangeOrderStatusCode = entity.ExchangeOrderStatusCode,
					lastErrorDate = entity.LastErrorDate,
					lastError = entity.LastError,

					updateRequired = entity.UpdateRequired,
					cancelRequired = entity.CancelRequired,
					statusDescription = entity.StatusDescription,
					id = entity.Id
				}, transaction);
		}

		public Task<Order> Update (ExchangeOrder entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"
					update
						Orders
					set
						orderStatusCode = @orderStatusCode,
						exchangeOrderId = @exchangeOrderId,
						updated = @updated,
					where
						id = @id;
					returning *;
				";

			return connection.QueryFirstAsync<Order>(query,
				new
				{
					orderStatusCode = entity.Status.Code,
					exchangeOrderId = entity.ExchangeOrderId,
					updated = entity.TransactTime
				}, transaction);
		}
	}
}
