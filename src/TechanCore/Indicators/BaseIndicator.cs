namespace TechanCore.Indicators
{
	public abstract class BaseIndicator<T, TK> : IIndicator<T, TK> where T : IOptionsSet where TK: IResultSet
	{
		public abstract string Name { get; }

		public abstract TK Get (ICandle[] source, T options);
		public abstract TK Get(decimal[] source, T options);
		public abstract TK Get(decimal?[] source, T options);
	}
}
