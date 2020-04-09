using Abstractions.Enums;

namespace Abstractions.Entities
{
    public interface IAccount
    {
        Assets Asset { get; set; }
        decimal AvailableAmount { get; set; }
        decimal LockedAmount { get; set; }
    }
}