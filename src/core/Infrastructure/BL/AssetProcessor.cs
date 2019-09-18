using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class AssetProcessor : BaseProcessor
	{
		private IAssetManager _assetManager;
		

		public AssetProcessor (IOptions<AppSettings> settings, ILogger<AssetProcessor> logger, IAssetManager assetManager) : base(settings, logger)
		{
			_assetManager = assetManager;
		}

		public Task<IEnumerable<Asset>> GetAll ()
		{
			return WithConnection((connection, transaction) => _assetManager.GetAll(connection, transaction));
		}

		public Task<long> Create (Asset entity)
		{
			return WithConnection((connection, transaction) => _assetManager.Create(entity, connection, transaction));
		}
	}
}
