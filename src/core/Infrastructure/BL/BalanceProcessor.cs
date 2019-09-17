using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class BalanceProcessor : BaseProcessor
	{
		private IBalanceManager _balanceManager;

		public BalanceProcessor (IOptions<AppSettings> settings, ILogger logger, IBalanceManager balanceManager) : base(settings, logger)
		{
			_balanceManager = balanceManager;
		}

		public async Task<Balance> UpdateOrCreate(Balance balance)
		{
			return await WithConnection(async (connection, transaction) => 
			{
				Balance storedBalance = await _balanceManager.Get(balance.Asset, balance.ExchangeCode, connection, transaction);
				if(storedBalance != null)
				{
					return await _balanceManager.Update(balance, connection, transaction);
				}
				else
				{
					long? id = await _balanceManager.Create(balance, connection, transaction);
					return await _balanceManager.Get(id.Value, connection, transaction);
				}
			});
		}

		public async Task<Balance> Get(long id)
		{
			return await WithConnection(async (connection, transaction) =>
			{
				return await _balanceManager.Get(id, connection, transaction);
			});
		}
	}
}
