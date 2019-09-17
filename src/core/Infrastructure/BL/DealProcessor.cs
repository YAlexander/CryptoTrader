using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class DealProcessor : BaseProcessor
	{
		private IDealManager _dealManager;

		public DealProcessor (IOptions<AppSettings> settings, ILogger logger, IDealManager dealManager) : base(settings, logger)
		{
			_dealManager = dealManager;
		}

		public Task<Deal> Update(Deal deal)
		{
			return WithConnection((connection, transaction) =>
			{
				return _dealManager.Update(deal, connection, transaction);
			});
		}

		public Task<Deal> Get(long id)
		{
			return WithConnection((connection, transaction) =>
			{
				return _dealManager.Get(id, connection, transaction);
			});
		}
	}
}
