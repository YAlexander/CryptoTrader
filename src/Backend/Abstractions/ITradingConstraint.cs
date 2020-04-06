using System.Threading.Tasks;

namespace Abstractions
{
	public interface ITradingConstraint
	{
		Task<ITradingContext> Set(ITradingContext context);
	}
}