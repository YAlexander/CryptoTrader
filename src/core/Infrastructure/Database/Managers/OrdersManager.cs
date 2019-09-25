using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using core.Infrastructure.Models;

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
						isDeleted,
						tradingModeCode,
						exchangeCode,
						orderSideCode,
						orderTypeCode,
						orderStatusCode,
						fillPoliticsCode,
						dealId,
						exchangeOrderId,
						exchangeOrderStatusCode,
						symbol,
						price,
						amount,
						limit,
						stopLimit,
						stopLoss,
						takeProfit,
						expirationDate,
						LastErrorDate,
						lastError,
						isUpdateRequired,
						isCancelRequired,
						statusDescription
					)
					vales
					(
						default,
						@created,
						@updated,
						false
						@tradingModeCode,
						@exchangeCode,
						@orderSideCode,
						@orderTypeCode,
						@orderStatusCode,
						@fillPoliticsCode,
						@dealId,
						@exchangeOrderId,
						@exchangeOrderStatusCode,
						@symbol,
						@price,
						@amount,
						@limit,
						@stopLimit,
						@stopLoss,
						@takeProfit,
						@expirationDate,
						@LastErrorDate,
						@lastError,
						@isUpdateRequired,
						@isCancelRequired,
						@statusDescription
					)
					returning
						id;";

			return connection.QuerySingleAsync<long>(query,
				new
				{
					created = entity.Created,
					updated = entity.Updated,
					tradingModeCode = entity.TradingModeCode,
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
					limit = entity.Limit,
					stopLimit = entity.StopLimit,
					stopLoss = entity.StopLoss,
					takeProfit = entity.TakeProfit,
					expirationDate = entity.ExpirationDate,
					LastErrorDate = entity.LastErrorDate,
					lastError = entity.LastError,
					isUpdateRequired = entity.UpdateRequired,
					isCancelRequired = entity.CancelRequired,
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
