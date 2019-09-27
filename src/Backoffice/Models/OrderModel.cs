using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backoffice.Models
{
	public class OrderModel
	{
		public int TradingMode { get; set; } = TradingModeCode.AUTO.Code;
		public int Exchange { get; set; } = ExchangeCode.UNKNOWN;

		public int OrderSide { get; set; } = OrderSideCode.BUY.Code;
		public int OrderType { get; set; } = OrderTypeCode.UNKNOWN.Code;
		public int OrderStatus { get; set; } = OrderStatusCode.PENDING.Code;
		public int FillPolitics { get; set; } = FillPoliticsCode.GTC.Code;

		public long? DealId { get; set; }
		public long? ExchangeOrderId { get; set; }
		public long? ExchangeOrderStatus { get; set; }
		public string StatusDescription { get; set; }

		public string Symbol { get; set; }

		// Order price
		public decimal? Price { get; set; }
		// Order amount
		public decimal? Amount { get; set; }

		// Price for new stop order
		public decimal? Limit { get; set; }

		// Trigger price for stop order
		public decimal? StopPrice { get; set; }

		public decimal? StopLoss { get; set; }

		public decimal? TakeProfit { get; set; }

		public DateTime? ExpirationDate { get; set; }
		public DateTime? LastErrorDate { get; set; }
		public string LastError { get; set; }

		public bool UpdateRequired { get; set; }
		public bool CancelRequired { get; set; }

		// Additional Data
		public List<Fill> Fills { get; set; } = new List<Fill>();

		public IEnumerable<SelectListItem> AllExchanges { get; } = ExchangeCode.Options.Select(x => new SelectListItem(text: x.Description, value: x.Code.ToString()));
		public IEnumerable<SelectListItem> AllFillPolitics { get; } = FillPoliticsCode.Options.Select(x => new SelectListItem(text: x.Description, value: x.Code.ToString()));
		public IEnumerable<SelectListItem> AllOrderStatuses { get; } = OrderStatusCode.Options.Select(x => new SelectListItem(text: x.Description, value: x.Code.ToString()));
		public IEnumerable<SelectListItem> AllOrderTypes { get; } = OrderTypeCode.Options.Select(x => new SelectListItem(text: x.Description, value: x.Code.ToString()));
		public IEnumerable<SelectListItem> AllOrderSides { get; } = OrderSideCode.Options.Select(x => new SelectListItem(text: x.Description, value: x.Code.ToString()));
	}
}
