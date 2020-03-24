using System.Threading.Tasks;
using Abstractions.Enums;

namespace Persistence
{
	public interface IInstaller
	{
		Task CreateClusterTables (string connectionString);

		Task CreateCryptoTraderTables (string connectionString);

		Task CreateExchangeSchema(Exchanges exchange);

		Task AddPair(Exchanges exchange, Assets asset1, Assets asset2);
	}
}