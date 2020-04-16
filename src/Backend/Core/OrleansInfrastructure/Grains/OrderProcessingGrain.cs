using System;
using System.Threading.Tasks;
using Abstractions.Entities;
using Abstractions.Grains;
using Common;
using Core.BusinessLogic;
using Orleans;
using Orleans.Streams;
using Persistence.Entities;
using Persistence.Helpers;

namespace Core.OrleansInfrastructure.Grains
{
	[ImplicitStreamSubscription(nameof(TradingContext))]
	public class OrderProcessingGrain : Grain, IOrderProcessingGrain
	{
		private IStreamProvider _streamProvider;
		
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
			order.Id = Guid.NewGuid();

			GrainKeyExtension key = order.ToExtendedKey();
			key.Id ??= Guid.NewGuid();

			IOrderGrain newOrder = GrainFactory.GetGrain<IOrderGrain>(order.Id, key.ToString());
			await newOrder.Update(order);

			
			// TODO: Nats queue
			//INotification<IOrder> notification = new Notification<IOrder>();
			//notification.Payload = order;

			//await this.SendUpdateMessage(notification);
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
			GrainKeyExtension extension = order.ToExtendedKey();
			extension.Id ??= Guid.NewGuid();

			IOrderGrain grain = GrainFactory.GetGrain<IOrderGrain>(order.Id, extension.ToString());
			await grain.Update(order);
		}
	}
}