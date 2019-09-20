using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class PairConfigProcessor : BaseProcessor
	{
		private IPairConfigManager _pairConfigManager;

		public PairConfigProcessor (IOptions<AppSettings> settings, ILogger<PairConfigProcessor> logger, IPairConfigManager pairConfigManager) : base(settings, logger)
		{
			_pairConfigManager = pairConfigManager;
		}

		public Task<long> Create (PairConfig entity)
		{
			return WithConnection((connection, transaction) =>
			{
				return _pairConfigManager.Create(entity, connection, transaction);
			});
		}

		public Task<PairConfig> Update (PairConfig entity)
		{
			return WithConnection((connection, transaction) =>
			{
				return _pairConfigManager.Update(entity, connection, transaction);
			});
		}

		public Task<PairConfig> Get (long id)
		{
			return WithConnection((connection, transaction) =>
			{
				return _pairConfigManager.Get(id, connection, transaction);
			});
		}

		public Task<IEnumerable<PairConfig>> GetAssigned (long id)
		{
			return WithConnection((connection, transaction) =>
			{
				return _pairConfigManager.GetAssignedPairs(id, connection, transaction);
			});
		}
	}
}
