namespace Contracts.Trading
{
	public interface IStrategyOption
	{
		T GetOptions<T>() where T: class, new();
	}
}