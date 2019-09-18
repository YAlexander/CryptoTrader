using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Threading.Tasks;

namespace core.Infrastructure.Database.Managers
{
	public class AssetManager : IAssetManager
	{
		public Task<long> Create (Asset entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"
					insert into
						Assets
						(
							id,
							created,
							isDeleted,
							code,
							name,
							isEnabled
						)
					values
						(
							default,
							@created,
							false,
							@code,
							@name,
							true
						)
					returning id;
			";

			return connection.QuerySingleAsync<long>(query,
				new
				{
					created = entity.Created,
					code = entity.Code,
					name = entity.Name
				}, transaction);
		}

		public Task Delete (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"update Assets set isDeleted = true where id = @id";
			return connection.ExecuteAsync(query, new { id = id }, transaction);
		}

		public Task<Asset> Get (long id, IDbConnection connection, IDbTransaction transaction = null)
		{
			const string query = @"select * from Assets where id = @id";
			return connection.QuerySingleAsync<Asset>(query, new { id = id }, transaction);
		}

		public Task<IEnumerable<Asset>> GetAll (IDbConnection connection, IDbTransaction transaction)
		{
			const string query = @"select * from Assets";
			return connection.QueryAsync<Asset>(query, new { }, transaction);
		}

		public Task<Asset> Update (Asset entity, IDbConnection connection, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}
	}
}
