using System;
using Contracts.Trading;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains.GrainStates
{
	[Serializable]
	public class OrderState : IOrder, IEntity 
	{
		public long Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
	}
}