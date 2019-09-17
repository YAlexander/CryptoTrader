using core.Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Models.Mappers
{
	public static class PairMapper
	{
		public static PairConfigModel ToModel (this PairConfig entity)
		{
			PairConfigModel model = new PairConfigModel();
			model.Id = entity.Id;
			model.ExchangeCode = entity.ExchangeCode;
			model.Symbol = entity.Symbol;
			model.IsEnabled = entity.IsEnabled;
			model.StrategyId = entity.StrategyId;
			model.RefreshDelaySeconds = entity.RefreshDelaySeconds;
			model.DefaultStopLossPercent = entity.DefaultStopLossPercent;
			model.DefaultTakeProfitPercent = entity.DefaultTakeProfitPercent;
			model.IsTestMode = entity.IsTestMode;

			return model;
		}

		public static PairConfig ToEntity (this PairConfigModel model)
		{
			PairConfig entity = new PairConfig();
			entity.Id = model.Id;
			entity.ExchangeCode = model.ExchangeCode;
			entity.Symbol = model.Symbol;
			entity.IsEnabled = model.IsEnabled;
			entity.StrategyId = model.StrategyId;
			entity.RefreshDelaySeconds = model.RefreshDelaySeconds;
			entity.DefaultStopLossPercent = model.DefaultStopLossPercent;
			entity.DefaultTakeProfitPercent = model.DefaultTakeProfitPercent;
			entity.IsTestMode = model.IsTestMode;

			return entity;
		}
	}
}
