namespace TechanCore
{
	public interface IIndicator<in T, out TK> where T : IOptionsSet where TK : IResultSet
	{
		string Name { get; }

		TK Get (ICandle[] source, T options);
		TK Get (decimal[] source, T options);
		TK Get (decimal?[] source, T options);
	}
}
