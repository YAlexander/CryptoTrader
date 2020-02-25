using System;
using Contracts.Enums;
using Contracts.Trading;
using Persistence.Entities;

namespace Core.OrleansInfrastructure.Grains.GrainStates
{
	[Serializable]
	public class TradeState : ITradeInfo, IEntity
	{
		public DateTime OpenDate { get; set; }
		public DateTime CloseDate { get; set; }
		public TradeStatus Status { get; set; }
		public Exchanges Exchange { get; set; }
		public Assets Asset1 { get; set; }
		public Assets Asset2 { get; set; }
		public TradePositions TradePosition { get; set; }
		public decimal? BidPrice { get; set; }
		public decimal? AskPrice { get; set; }
		public decimal? Volume { get; set; }
		public long? StrategyId { get; set; }
		public long?[] OrderIds { get; set; }
		public long Id { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
	}
}