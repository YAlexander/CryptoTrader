using System;
using System.Threading.Tasks;
using Abstractions;
using Common;
using Contracts.Trading;
using Orleans;
using Orleans.Concurrency;
using Orleans.Streams;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains
{
	[StatelessWorker]
	[ImplicitStreamSubscription(nameof(ITradingContext))]
	public class OrderProcessingGrain : Grain, IOrderProcessingGrain, IAsyncObserver<ITradingContext>
	{
		private IStreamProvider _streamProvider;
		private GrainObserverManager<IOrderNotificator> _subsManager;

		public override async Task OnActivateAsync()
		{
			_streamProvider = GetStreamProvider("SMSProvider");
			IAsyncStream<ITradingContext> stream = _streamProvider.GetStream<ITradingContext>(this.GetPrimaryKey(), nameof(ITradingContext));
			await stream.SubscribeAsync(OnNextAsync);
			
			_subsManager = new GrainObserverManager<IOrderNotificator>();
			await base.OnActivateAsync();
		}

		public async Task OnNextAsync(ITradingContext context, StreamSequenceToken token = null)
		{
			// TODO: Process order

			Order order = new Order();
			order.Exchange = context.Exchange;
			order.Asset1 = context.TradingPair.asset1;
			order.Asset2 = context.TradingPair.asset2;

			INotification<IOrder> notification = new Notification<IOrder>();
			notification.Payload = order;

			await this.SendUpdateMessage(notification);
		}
		
		private Task SendUpdateMessage(INotification<IOrder> message)
		{
			_subsManager.Notify(s => s.ReceiveMessage(message));
			return Task.CompletedTask;
		}

		public Task OnCompletedAsync()
		{
			return Task.CompletedTask;
		}

		public Task OnErrorAsync(Exception ex)
		{
			return Task.CompletedTask;
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