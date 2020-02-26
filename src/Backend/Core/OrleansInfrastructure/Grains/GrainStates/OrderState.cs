using System;
using System.Collections.Generic;
using Contracts.Enums;
using Contracts.Trading;

namespace Core.OrleansInfrastructure.Grains.GrainStates
{
	[Serializable]
	public class OrderState : IOrder 
	{
		public string ExternalId { get; set; }

		public Exchanges Exchange { get; set; }

		public OrderSides Operation { get; set; }

		public OrderType Type { get; set; }

		public OrderStatus Status { get; set; }

		public string LastError { get; set; }

		public FillPolitics FillPolitics { get; set; }

		public string Symbol { get; set; }

		public decimal? Price { get; set; }
		public decimal? Amount { get; set; }

		public decimal? StopLoss { get; set; }
		public decimal? TakeProfit { get; set; }
		public decimal? StopPrice { get; set; }
		public DateTime? Expired { get; set; }

		public string Comment { get; set; }

		public IEnumerable<Fill> Fills { get; set; }

	}
}