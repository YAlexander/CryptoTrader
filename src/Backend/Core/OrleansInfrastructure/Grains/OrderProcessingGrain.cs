using System;
using System.Threading.Tasks;
using Abstractions;
using Abstractions.Entities;
using Abstractions.Grains;
using Common;
using Core.BusinessLogic;
using Orleans;
using Orleans.Streams;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains
{
	[ImplicitStreamSubscription(nameof(TradingContext))]
	public class OrderProcessingGrain : Grain, IOrderProcessingGrain
	{
		private IStreamProvider _streamProvider;
		private readonly GrainObserverManager<IOrderNotificator> _subsManager;

		public OrderProcessingGrain(GrainObserverManager<IOrderNotificator> subsManager)
		{
			_subsManager = subsManager;
		}

		public override async Task OnActivateAsync()
		{
			_streamProvider = GetStreamProvider(Constants.MessageStreamProvider);
			IAsyncStream<TradingContext> stream = _streamProvider.GetStream<TradingContext>(this.GetPrimaryKey(), nameof(TradingContext));
			await stream.SubscribeAsync(OnNextAsync);
		}

		private async Task OnNextAsync(TradingContext context, StreamSequenceToken token = null)
		{
			// TODO: Process order

			IOrder order = new Order();
			GrainKeyExtension key = new GrainKeyExtension();

			order.OrderId = Guid.NewGuid();
			key.Id = order.OrderId;
			order.Exchange = key.Exchange = context.Exchange;
			order.Asset1 = key.Asset1 = context.TradingPair.asset1;
			order.Asset2 = key.Asset2 = context.TradingPair.asset2;
			
			IOrderGrain newOrder = GrainFactory.GetGrain<IOrderGrain>(key.Id.Value, key.ToString());
			await newOrder.Update(order);
			
			INotification<IOrder> notification = new Notification<IOrder>();
			notification.Payload = order;

			await this.SendUpdateMessage(notification);
		}
		
		private async Task SendUpdateMessage(INotification<IOrder> message)
		{
			await _subsManager.Notify(s => s.ReceiveMessage(message));
		}

		public Task OnCompletedAsync()
		{
			return Task.CompletedTask;
		}

		public Task OnErrorAsync(Exception ex)
		{
			return Task.CompletedTask;
		}

		public async Task Update(IOrder order)
		{
			GrainKeyExtension extension = new GrainKeyExtension();
			extension.Id = order.OrderId;
			extension.Exchange = order.Exchange;
			extension.Asset1 = order.Asset1;
			extension.Asset2 = order.Asset2;
			extension.Id = order.OrderId;

			IOrderGrain grain = GrainFactory.GetGrain<IOrderGrain>(order.OrderId, extension.ToString());
			await grain.Update(order);
		}

		public Task Subscribe(IOrderNotificator observer)
		{
			_subsManager.Subscribe(observer);
			return Task.CompletedTask;
		}

		public Task UnSubscribe(IOrderNotificator observer)
		{ 
			_subsManager.Unsubscribe(observer);
			return Task.CompletedTask;
		}
	}
}