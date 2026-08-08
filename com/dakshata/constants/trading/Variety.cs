namespace com.dakshata.constants.trading
{
    /// <summary>
    /// Represents order variety.
    /// <para>
    /// BO and CO are your broker's own bracket and cover orders, so they are available only on
    /// brokers that offer them. AT_BO and AT_CO are AutoTrader's own bracket and cover orders,
    /// which work with every broker we support.
    /// </para>
    /// </summary>
    public enum Variety
    {
        REGULAR,
        BO,
        CO,
        AT_BO,
        AT_CO
    }
}