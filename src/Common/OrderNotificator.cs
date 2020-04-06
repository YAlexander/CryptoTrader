using System.Threading.Tasks;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Grains;
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
			IOrderProcessingGrain grain = _client.GetGrain<IOrderProcessingGrain>((int)order.Exchange);

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