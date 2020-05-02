using System.Threading.Tasks;

namespace Abstractions
{
	public interface INotificator<INotificatorOptions>
	{
		Task Notify(IMessage message);
	}
}
