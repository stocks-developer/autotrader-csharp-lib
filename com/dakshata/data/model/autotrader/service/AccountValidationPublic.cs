using com.dakshata.constants.trading;

namespace com.dakshata.data.model.autotrader.service
{
    /// <summary>
    /// Result of validating one trading account, as returned by ValidateAllAccounts.
    /// </summary>
    public class AccountValidationPublic
    {
        /// <summary>The trading account that was checked.</summary>
        public long? TradingAccId { get; set; }

        /// <summary>The pseudo account the trading account belongs to.</summary>
        public long? PseudoAccId { get; set; }

        /// <summary>Whether the account could log in to the broker.</summary>
        public bool Valid { get; set; }

        /// <summary>Outcome of the check.</summary>
        public TradingOpResult Result { get; set; }

        /// <summary>Why the check failed, when it did.</summary>
        public string Message { get; set; }

        /// <summary>The broker login id that was checked.</summary>
        public string TradingAccLoginId { get; set; }

        /// <summary>
        /// Session state at the broker: MISSING, LOGGED_IN, LOGGED_OUT or ERROR.
        /// A string rather than an enum, to stay readable if the platform adds a new state.
        /// </summary>
        public string SessionState { get; set; }

        public override string ToString()
        {
            return string.Format(
                "AccountValidationPublic [TradingAccLoginId={0}, Valid={1}, Result={2}, "
                    + "SessionState={3}, Message={4}, TradingAccId={5}]",
                TradingAccLoginId, Valid, Result, SessionState, Message, TradingAccId);
        }
    }

}
