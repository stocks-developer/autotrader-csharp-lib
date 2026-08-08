namespace com.dakshata.data.model.autotrader.service
{
    /// <summary>
    /// A trading account under your user, as returned by FetchAllTradingAccounts.
    /// Never carries credentials or any other sensitive field.
    /// </summary>
    public class TradingAccountPublic
    {
        /// <summary>Your login id at the broker.</summary>
        public string LoginId { get; set; }

        /// <summary>The nickname (pseudo account) this account trades under.</summary>
        public string PseudoAccName { get; set; }

        /// <summary>The stock broker.</summary>
        public string Broker { get; set; }

        /// <summary>The platform used to connect this account to the broker.</summary>
        public string Platform { get; set; }

        /// <summary>Licence expiry date. An expired account cannot trade.</summary>
        public string LicenseExpiryDate { get; set; }

        /// <summary>Whether the account is live. Non-live accounts are not available for trading.</summary>
        public bool? Live { get; set; }

        /// <summary>Our internal id for this trading account, used by validate account.</summary>
        public long? SystemId { get; set; }

        /// <summary>Our internal id for the pseudo account this trading account belongs to.</summary>
        public long? SystemIdOfPseudoAcc { get; set; }

        /// <summary>Days remaining on the licence.</summary>
        public int? LicenseDaysLeft { get; set; }

        public override string ToString()
        {
            return string.Format(
                "TradingAccountPublic [LoginId={0}, PseudoAccName={1}, Broker={2}, Platform={3}, "
                    + "Live={4}, LicenseExpiryDate={5}, LicenseDaysLeft={6}, SystemId={7}]",
                LoginId, PseudoAccName, Broker, Platform, Live, LicenseExpiryDate, LicenseDaysLeft,
                SystemId);
        }
    }

}
