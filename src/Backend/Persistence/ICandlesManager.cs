using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Contracts.Enums;
using Persistence.Entities;

namespace Persistence
{
	public interface ICandlesManager
	{
		public Task<IEnumerable<Candle>> Get(Exchanges exchange, Assets asset1, Assets asset2, DateTime from, DateTime to, Timeframes period, IDbConnection connection, IDbTransaction transaction = null);
	}
}