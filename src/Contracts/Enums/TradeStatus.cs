namespace Contracts.Enums
{
    public enum TradeStatus
    {
        /// <summary>
        /// When first order places
        /// </summary>
        PENDING,
        
        /// <summary>
        /// Order is in OrdersBook
        /// </summary>
        ACTIVE,
        
        /// <summary>
        /// Successfully finished
        /// </summary>
        CLOSED,
        
        /// <summary>
        /// Cancelled
        /// </summary>
        CANCELLED
    }
}