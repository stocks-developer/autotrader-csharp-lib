using com.dakshata.constants.trading;
using com.dakshata.data.model.common;
using com.dakshata.trading.model.platform;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace com.dakshata.autotrader.api
{
    public class AutoTrader : IAutoTrader
    {

        public const string PRIMARY_SERVER_URL = "https://apix.stocksdeveloper.in";
        
        public const string BACKUP_SERVER_URL = "https://api.stocksdeveloper.in";

        public const string SERVER_URL = PRIMARY_SERVER_URL;

        private const string GET = "GET", POST = "POST";

        private const string TRADING_URI = "/trading";

        private const string ACCOUNT_URI = "/account";

        private static readonly IDictionary<string, AutoTrader> INSTANCES =
            new ConcurrentDictionary<string, AutoTrader>();

        private readonly string apiKey, serviceUrl;

        private static readonly JsonSerializerOptions JSON_OPTIONS =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true
            };

        static AutoTrader()
        {
            JSON_OPTIONS.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }

        /// <summary>
        /// Initialize the AutoTrader API with your private API key.
        /// </summary>
        /// <param name="apiKey">     your private api key </param>
        /// <param name="serviceUrl"> AutoTrader api service url </param>
        private AutoTrader(string apiKey, string serviceUrl) : base()
        {
            this.apiKey = apiKey;
            this.serviceUrl = serviceUrl;
        }

        public static IAutoTrader CreateInstance(string apiKey, string serviceUrl)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new System.ArgumentException("API Key cannot be null", "apiKey");
            }
            if (string.IsNullOrEmpty(serviceUrl))
            {
                throw new System.ArgumentException("Service url cannot be null", "serviceUrl");
            }

            if (INSTANCES.ContainsKey(apiKey))
            {
                return INSTANCES[apiKey];
            }

            lock (typeof(AutoTrader))
            {
                if (INSTANCES.ContainsKey(apiKey))
                {
                    return INSTANCES[apiKey];
                }
                else
                {
                    AutoTrader instance = new AutoTrader(apiKey, serviceUrl);
                    INSTANCES[apiKey] = instance;
                    return instance;
                }

            }
        }

        public IOperationResponse<ISet<string>> FetchLivePseudoAccounts()
        {
            return Execute<ISet<string>>(GET, ACCOUNT_URI + "/fetchLivePseudoAccounts");
        }

        public IOperationResponse<bool?> CancelAllOrders(string pseudoAccount)
        {
            return CancelGeneric("/cancelAllOrders", pseudoAccount, null);
        }

        public IOperationResponse<bool?> CancelOrderByPlatformId(string pseudoAccount,
            string platformId)
        {
            return CancelGeneric("/cancelOrderByPlatformId", pseudoAccount, platformId);
        }

        public IOperationResponse<bool?> CancelChildOrdersByPlatformId(string pseudoAccount,
            string platformId)
        {
            return CancelGeneric("/cancelChildOrdersByPlatformId", pseudoAccount, platformId);
        }

        public IOperationResponse<bool?> ModifyOrderByPlatformId(string pseudoAccount,
            string platformId, OrderType? orderType, int? quantity, float? price,
            float? triggerPrice)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["pseudoAccount"] = pseudoAccount;
            data["platformId"] = platformId;
            if (orderType != null)
                data["orderType"] = Enum.GetName(typeof(OrderType), orderType);
            if (quantity != null)
                data["quantity"] = quantity;
            if (price != null)
                data["price"] = price;
            if (triggerPrice != null)
                data["triggerPrice"] = triggerPrice;

            return Execute<bool?>(POST, TRADING_URI + "/modifyOrderByPlatformId", data);
        }

        public IOperationResponse<string> PlaceBracketOrder(string pseudoAccount,
            string exchange, string symbol, TradeType tradeType, OrderType orderType,
            int quantity, float price, float triggerPrice,
            float target, float stoploss, float trailingStoploss)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["pseudoAccount"] = pseudoAccount,
                ["exchange"] = exchange,
                ["symbol"] = symbol,
                ["tradeType"] = tradeType,
                ["orderType"] = orderType,
                ["quantity"] = quantity,
                ["price"] = price,
                ["triggerPrice"] = triggerPrice,
                ["target"] = target,
                ["stoploss"] = stoploss,
                ["trailingStoploss"] = trailingStoploss
            };

            return Execute<string>(POST, TRADING_URI + "/placeBracketOrder", data);
        }

        public IOperationResponse<string> PlaceCoverOrder(string pseudoAccount,
            string exchange, string symbol, TradeType tradeType, OrderType orderType,
            int quantity, float price, float triggerPrice)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["pseudoAccount"] = pseudoAccount,
                ["exchange"] = exchange,
                ["symbol"] = symbol,
                ["tradeType"] = tradeType,
                ["orderType"] = orderType,
                ["quantity"] = quantity,
                ["price"] = price,
                ["triggerPrice"] = triggerPrice
            };

            return Execute<string>(POST, TRADING_URI + "/placeCoverOrder", data);
        }

        public IOperationResponse<string> PlaceAutoTraderBracketOrder(string pseudoAccount,
            string exchange, string symbol, TradeType tradeType, OrderType orderType,
            int quantity, float price, float triggerPrice,
            float target, float stoploss, float trailingStoploss)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["pseudoAccount"] = pseudoAccount,
                ["exchange"] = exchange,
                ["symbol"] = symbol,
                ["tradeType"] = tradeType,
                ["orderType"] = orderType,
                ["quantity"] = quantity,
                ["price"] = price,
                ["triggerPrice"] = triggerPrice,
                ["target"] = target,
                ["stoploss"] = stoploss,
                ["trailingStoploss"] = trailingStoploss
            };

            return Execute<string>(POST, TRADING_URI + "/placeAutoTraderBracketOrder", data);
        }

        public IOperationResponse<string> PlaceAutoTraderCoverOrder(string pseudoAccount,
            string exchange, string symbol, TradeType tradeType, OrderType orderType,
            int quantity, float price, float stoploss, float trailingStoploss)
        {
            // No triggerPrice: an AutoTrader cover order carries its stop as a distance in
            // rupees in stoploss, unlike a broker cover order which uses triggerPrice as the
            // stop price.
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["pseudoAccount"] = pseudoAccount,
                ["exchange"] = exchange,
                ["symbol"] = symbol,
                ["tradeType"] = tradeType,
                ["orderType"] = orderType,
                ["quantity"] = quantity,
                ["price"] = price,
                ["stoploss"] = stoploss,
                ["trailingStoploss"] = trailingStoploss
            };

            return Execute<string>(POST, TRADING_URI + "/placeAutoTraderCoverOrder", data);
        }

        public IOperationResponse<string> PlaceRegularOrder(string pseudoAccount,
            string exchange, string symbol, TradeType tradeType, OrderType orderType,
            ProductType productType, int quantity, float price, float triggerPrice)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["pseudoAccount"] = pseudoAccount,
                ["exchange"] = exchange,
                ["symbol"] = symbol,
                ["tradeType"] = tradeType,
                ["orderType"] = orderType,
                ["productType"] = productType,
                ["quantity"] = quantity,
                ["price"] = price,
                ["triggerPrice"] = triggerPrice
            };

            return Execute<string>(POST, TRADING_URI + "/placeRegularOrder", data);
        }

        public IOperationResponse<string> PlaceAdvancedOrder(Variety variety, string pseudoAccount, 
            string exchange, string symbol, TradeType tradeType, OrderType orderType, ProductType productType, 
            int quantity, float price, float triggerPrice, float target, float stoploss, float trailingStoploss, 
            int disclosedQuantity, Validity validity, bool amo, string strategyId, string comments, string publisherId)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["variety"] = variety,
                ["pseudoAccount"] = pseudoAccount,
                ["exchange"] = exchange,
                ["symbol"] = symbol,
                ["tradeType"] = tradeType,
                ["orderType"] = orderType,
                ["productType"] = productType,
                ["quantity"] = quantity,
                ["price"] = price,
                ["triggerPrice"] = triggerPrice,
                ["target"] = target,
                ["stoploss"] = stoploss,
                ["trailingStoploss"] = trailingStoploss,
                ["disclosedQuantity"] = disclosedQuantity,
                ["validity"] = validity,
                ["amo"] = amo,
                ["strategyId"] = strategyId,
                ["comments"] = comments,
                ["publisherId"] = publisherId
            };

            return Execute<string>(POST, TRADING_URI + "/placeAdvancedOrder", data);
        }

        public IOperationResponse<bool?> SquareOffPosition(string pseudoAccount,
            PositionCategory category, PositionType type, string exchange, string symbol)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["pseudoAccount"] = pseudoAccount,
                ["category"] = category,
                ["type"] = type,
                ["exchange"] = exchange,
                ["symbol"] = symbol
            };

            return Execute<bool?>(POST, TRADING_URI + "/squareOffPosition", data);
        }

        public IOperationResponse<bool?> SquareOffPortfolio(string pseudoAccount,
            PositionCategory category)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["pseudoAccount"] = pseudoAccount,
                ["category"] = category
            };

            return Execute<bool?>(POST, TRADING_URI + "/squareOffPortfolio", data);
        }

        public IOperationResponse<ISet<PlatformMargin>> ReadPlatformMargins(string pseudoAccount)
        {
            return Read<PlatformMargin>(pseudoAccount, "/readPlatformMargins");
        }

        public IOperationResponse<ISet<PlatformOrder>> ReadPlatformOrders(string pseudoAccount)
        {
            return Read<PlatformOrder>(pseudoAccount, "/readPlatformOrders");
        }

        public IOperationResponse<ISet<PlatformPosition>> ReadPlatformPositions(string pseudoAccount)
        {
            return Read<PlatformPosition>(pseudoAccount, "/readPlatformPositions");
        }

        public IOperationResponse<ISet<PlatformHolding>> ReadPlatformHoldings(string pseudoAccount)
        {
            return Read<PlatformHolding>(pseudoAccount, "/readPlatformHoldings");
        }

        public void Shutdown()
        {
            // Nothing needed here
        }

        private IOperationResponse<T> Execute<T>(string method, string uri,
            IDictionary<string, object> data = null)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.serviceUrl + uri);
            request.Method = method;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers["api-key"] = this.apiKey;

            if (data != null && data.Count > 0)
            {
                string postData = "";
                foreach (string key in data.Keys)
                {
                    object paramValue = data[key];
                    string paramText = (paramValue == null) ? "" : paramValue.ToString();
                    postData += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(paramText) + "&";
                }

                byte[] bytes = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = bytes.Length;
                request.ContentType = "application/x-www-form-urlencoded";

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(bytes, 0, bytes.Length);
                dataStream.Close();
            }


            var content = string.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                return new OperationResponse<T>(default, ex, null, null);
            }

            return JsonSerializer.Deserialize<OperationResponse<T>>(content, JSON_OPTIONS);
        }

        private IOperationResponse<bool?> CancelGeneric(string uri, string pseudoAccount,
            string platformId)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "pseudoAccount", pseudoAccount }
            };

            if (platformId != null)
            {
                data["platformId"] = platformId;
            }

            return Execute<bool?>(POST, TRADING_URI + uri, data);
        }

        public IOperationResponse<ISet<T>> Read<T>(string pseudoAccount, string uri)
        {
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                ["pseudoAccount"] = pseudoAccount,
            };

            return Execute<ISet<T>>(POST, TRADING_URI + uri, data);
        }

    }
}
