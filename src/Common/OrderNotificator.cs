using System.Threading.Tasks;
using Abstractions;
using Contracts.Trading;
using Persistence;
using Persistence.Entities;

namespace Common
{
	public class OrderNotificator : IOrderNotificator
	{
		private readonly IExchangeOrderProcessor _orderProcessor;
		
		public OrderNotificator(IExchangeOrderProcessor orderProcessor)
		{
			_orderProcessor = orderProcessor;
		}
		
		public async Task ReceiveMessage(INotification<IOrder> notification)
		{
			IOrder order = notification.Payload;

			if (order.CreateRequired)
			{
				await _orderProcessor.PlaceOrder(order);
			}
			else if(order.UpdateRequired)
			{
				await _orderProcessor.UpdateOrder(order);
			}
			else
			{
				await _orderProcessor.CancelOrder(order.ExchangeOrderId);
			}
		}
	}
}