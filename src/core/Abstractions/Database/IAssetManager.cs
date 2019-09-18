using core.Infrastructure.Database.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace core.Abstractions.Database
{
	public interface IAssetManager : IDatabaseManager<Asset>
	{
		Task<IEnumerable<Asset>> GetAll (IDbConnection connection, IDbTransaction transaction);
	}
}
