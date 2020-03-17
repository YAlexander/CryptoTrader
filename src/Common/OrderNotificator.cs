using System.Threading.Tasks;
using Abstractions;
using Contracts.Trading;
using Orleans;

namespace Common
{
	public class OrderNotificator : IOrderNotificator
	{
		private readonly IClusterClient _client;
		private readonly IExchangeOrderProcessor _orderProcessor;
		
		public OrderNotificator(IExchangeOrderProcessor orderProcessor, IClusterClient client)
		{
			_orderProcessor = orderProcessor;
			_client = client;
		}
		
		public async Task ReceiveMessage(INotification<IOrder> notification)
		{
			IOrder order = notification.Payload;
			INotificationGrain grain = _client.GetGrain<INotificationGrain>((int)order.Exchange);

			if (order.CreateRequired)
			{
				order.ExchangeOrderId = await _orderProcessor.PlaceOrder(order);
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