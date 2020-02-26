using Contracts.Enums;

namespace Contracts.Trading
{
    public interface IAccount
    {
        Assets Asset { get; set; }
        decimal AvailableAmount { get; set; }
        decimal LockedAmount { get; set; }
    }
}