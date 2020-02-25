using core.Infrastructure.Database.Entities;
using core.TypeCodes;

namespace Backoffice.Models.Mappers
{
	public static class StrategyMapper
	{
		public static StrategyModel ToModel (this Strategy entity)
		{
			StrategyModel model = new StrategyModel();
			model.Id = entity.Id;
			model.Name = entity.Name;
			model.OptimalPeriodCode = PeriodCode.Create(entity.OptimalTimeframe).Code;
			model.OptimalPeriodName = PeriodCode.Create(entity.OptimalTimeframe).Description;
			model.NumberOfCandles = entity.NumberOfCandles;
			model.IsEnabled = entity.IsEnabled;

			return model;
		}

		public static Strategy ToEntity (this StrategyModel model)
		{
			Strategy entity = new Strategy();
			entity.Name = model.Name;
			entity.OptimalTimeframe = model.OptimalPeriodCode;
			entity.NumberOfCandles = model.NumberOfCandles;
			entity.Id = model.Id;
			entity.IsEnabled = model.IsEnabled;

			return entity;
		}
	}
}
