using Abstractions;
using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;

namespace Core.OrleansInfrastructure.Grains
{
	[StatelessWorker]
	public class OrderRProcessingGrain : Grain, IOrderProcessingGrain
	{
	}
}