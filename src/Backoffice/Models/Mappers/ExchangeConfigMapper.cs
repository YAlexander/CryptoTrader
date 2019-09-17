using core.Infrastructure.Database.Entities;
using core.TypeCodes;
using System;
using System.Linq;

namespace Backoffice.Models.Mappers
{
	public static class ExchangeConfigMapper
	{
		public static ExchangeConfig ToEntity(this ExchangeConfigModel model)
		{
			ExchangeConfig entity = new ExchangeConfig();
			entity.Id = model.Id ?? 0;
			entity.Updated = DateTime.UtcNow;
			entity.ExchangeCode = model.ExchangeCodeId;
			entity.IsEnabled = model.IsEnabled;
			entity.ApiKey = model.ApiKey;
			entity.ApiSecret = model.ApiSecret;

			return entity;
		}

		public static ExchangeConfigModel ToModel (this ExchangeConfig entity)
		{
			ExchangeConfigModel model = new ExchangeConfigModel();
			model.Id = entity.Id;
			model.ExchangeCodeId = entity.ExchangeCode;
			model.ExchangeName = ExchangeCode.Create(entity.ExchangeCode).Description;
			model.IsEnabled = entity.IsEnabled;
			model.ApiKey = entity.ApiKey;
			model.ApiSecret = entity.ApiSecret;
			model.Pairs = entity.Pairs.Select(x => x.ToModel());
			return model;
		}
	}
}
