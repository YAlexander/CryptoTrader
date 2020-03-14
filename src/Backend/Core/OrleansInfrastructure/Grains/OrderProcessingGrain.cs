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
		private GrainObserverManager<INotificator> _subsManager;

		public override async Task OnActivateAsync()
		{
			_streamProvider = GetStreamProvider("SMSProvider");
			IAsyncStream<ITradingContext> stream = _streamProvider.GetStream<ITradingContext>(this.GetPrimaryKey(), nameof(ITradingContext));
			await stream.SubscribeAsync(OnNextAsync);
			
			_subsManager = new GrainObserverManager<INotificator>();
			await base.OnActivateAsync();
		}

		public async Task OnNextAsync(ITradingContext item, StreamSequenceToken token = null)
		{
			// TODO: Process order

			Order order = new Order();
			order.Exchange = item.Exchange;
			order.Asset1 = item.TradingPair.asset1;
			order.Asset2 = item.TradingPair.asset2;

			INotification notification = new Notification();
			notification.Payload = order;

			await this.SendUpdateMessage(notification);
		}
		
		private Task SendUpdateMessage(INotification message)
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
		
		public Task Subscribe(INotificator observer)
		{
			_subsManager.Subscribe(observer);
			return Task.CompletedTask;
		}

		public Task UnSubscribe(INotificator observer)
		{ 
			_subsManager.Unsubscribe(observer);
			return Task.CompletedTask;
		}
	}
}