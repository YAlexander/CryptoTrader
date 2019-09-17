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

		public async Task<IEnumerable<Strategy>> GetAll ()
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _strategyManager.GetAll(connection, transaction);
			});
		}

		public async Task<bool> Enable(long id)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _strategyManager.Enable(id, connection, transaction);
			});
		}
	}
}
