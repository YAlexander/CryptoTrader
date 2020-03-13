using System;
using System.Threading.Tasks;
using Abstractions;
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

		public override async Task OnActivateAsync()
		{
			_streamProvider = GetStreamProvider("SMSProvider");
			IAsyncStream<ITradingContext> stream = _streamProvider.GetStream<ITradingContext>(this.GetPrimaryKey(), nameof(ITradingContext));
			await stream.SubscribeAsync(OnNextAsync);

			await base.OnActivateAsync();
		}

		public async Task OnNextAsync(ITradingContext item, StreamSequenceToken token = null)
		{
			Order order = new Order();
		}

		public Task OnCompletedAsync()
		{
			return Task.CompletedTask;
		}

		public Task OnErrorAsync(Exception ex)
		{
			return Task.CompletedTask;
		}
	}
}