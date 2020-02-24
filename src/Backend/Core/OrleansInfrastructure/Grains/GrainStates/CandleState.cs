using System;
using Contracts;
using Contracts.Enums;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains.GrainStates
{
	[Serializable]
	public class CandleState : ICandle, IEntity
	{
		public long Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }

		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }
		
		public DateTime Time { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Open { get; set; }
		public decimal Close { get; set; }
		public decimal Volume { get; set; }
		public decimal Trades { get; set; }
	}
}