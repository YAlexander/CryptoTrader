using System.Threading.Tasks;
using Abstractions;
using Contracts.Trading;
using Persistence;
using Persistence.Entities;

namespace Common
{
	public class OrderNotificator : IOrderNotificator
	{
		private readonly ISettingsProcessor _exchangeSettingsProcessor;

		public OrderNotificator(ISettingsProcessor exchangeSettingsProcessor)
		{
			_exchangeSettingsProcessor = exchangeSettingsProcessor;
		}
		
		public async Task ReceiveMessage(INotification<IOrder> notification)
		{
			IOrder order = notification.Payload;
			IExchangeSettings pairConfig = await _exchangeSettingsProcessor.Get(order.Exchange, order.Asset1, order.Asset2);
			
			
		}
	}
}