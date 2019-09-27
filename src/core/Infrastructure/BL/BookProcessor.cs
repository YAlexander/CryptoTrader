using core.Abstractions.Database;
using core.Infrastructure.Database.Entities;
using core.Infrastructure.Database.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace core.Infrastructure.BL
{
	public class BookProcessor : BaseProcessor
	{
		private IBookManager _bookManager;

		public BookProcessor (IOptions<AppSettings> settings, ILogger<BookProcessor> logger, IBookManager bookManager) : base(settings, logger)
		{
			_bookManager = bookManager;
		}

		public Task<long> Create (Book book)
		{
			return WithConnection((connection, transaction) =>
			{
				return _bookManager.Create(book, connection, transaction);
			});
		}

		public Task<Book> GetLast(int exchangeCode, string symbol)
		{
			return WithConnection((connection, transaction) =>
			{
				return _bookManager.GetLast(exchangeCode, symbol, connection, transaction);
			});
		}
	}
}
