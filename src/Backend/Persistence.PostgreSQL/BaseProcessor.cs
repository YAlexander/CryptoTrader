using System;
using System.Data;
using System.Threading.Tasks;
using Contracts;
using Npgsql;

namespace Persistence.PostgreSQL
{
	public abstract class BaseProcessor : IProcessor
	{
		private readonly string _connectionString;
		
		protected BaseProcessor (string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task<T> WithConnection<T> (Func<IDbConnection, IDbTransaction, Task<T>> f)
		{
			await using NpgsqlConnection sqlConnection = new NpgsqlConnection(_connectionString);
			await sqlConnection.OpenAsync();
			
			await using NpgsqlTransaction transaction = sqlConnection.BeginTransaction();
			try
			{
			 	T result = await f(sqlConnection, transaction);
			 	transaction.Commit();
			 		
			 	return result;
			}
			catch
			{
				await transaction.RollbackAsync();
			 	throw;
			}
		}
	}
}
