using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace core.Infrastructure.Database.Managers
{
	public abstract class BaseProcessor
	{
		private IOptions<AppSettings> _settings;
		private ILogger _logger;

		public BaseProcessor (IOptions<AppSettings> settings, ILogger logger)
		{
			_settings = settings;
			_logger = logger;
		}

		protected async Task<T> WithConnection<T> (Func<IDbConnection, IDbTransaction, Task<T>> f)
		{
			T result = default(T);
			using (NpgsqlConnection sqlConnection = new NpgsqlConnection(_settings.Value.ConnectionString))
			{
				await sqlConnection.OpenAsync();
				using (NpgsqlTransaction transaction = sqlConnection.BeginTransaction())
				{
					try
					{
						result = await f(sqlConnection, transaction);
						transaction.Commit();
					}
					catch (Exception ex)
					{
						_logger.LogError(ex.Message, ex);
						await transaction.RollbackAsync();
						throw;
					}
				}

				return result;
			}
		}
	}
}
