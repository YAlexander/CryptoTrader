using System.Threading.Tasks;

namespace Contracts.Trading
{
	public interface ITradingConstraint
	{
		Task<ITradingContext> Set(ITradingContext context);
	}
}