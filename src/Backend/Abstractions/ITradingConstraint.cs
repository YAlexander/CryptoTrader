using System.Threading.Tasks;
using Common.Trading;

namespace Abstractions
{
	public interface ITradingConstraint
	{
		Task<ITradingContext> Set(ITradingContext context);
	}
}