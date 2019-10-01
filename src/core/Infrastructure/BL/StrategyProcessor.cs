using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class StrategyProcessor : BaseProcessor
	{
		private IOptions<AppSettings> _settings;
		private IStrategyManager _strategyManager;

		public StrategyProcessor (
			ILogger<StrategyProcessor> logger,
			IOptions<AppSettings> settings,
			IStrategyManager strategyManager) : base(settings, logger)
		{
			_settings = settings;
			_strategyManager = strategyManager;
		}

		public Task<IEnumerable<Strategy>> GetAll ()
		{
			return WithConnection((connection, transaction) =>
			{
				return _strategyManager.GetAll(connection, transaction);
			});
		}

		public Task<bool> Enable(long id)
		{
			return WithConnection((connection, transaction) =>
			{
				return _strategyManager.Enable(id, connection, transaction);
			});
		}

		public Task<Strategy> Get (long id)
		{
			return WithConnection((connection, transaction) =>
			{
				return _strategyManager.Get(id, connection, transaction);
			});
		}
	}
}
