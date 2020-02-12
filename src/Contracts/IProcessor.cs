using System;
using System.Data;
using System.Threading.Tasks;

namespace Contracts
{
	public interface IProcessor
	{
		Task<T> WithConnection<T>(Func<IDbConnection, IDbTransaction, Task<T>> f);
	}
}