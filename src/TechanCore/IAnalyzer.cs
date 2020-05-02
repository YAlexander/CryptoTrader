namespace TechanCore
{
	public interface IAnalyzer<in T, out TK> where T : IOptionsSet where TK : IResultSet
	{
		TK Get(T options);
	}
}
