# AutoTrader Web C# Library: Broker-Independent Trading API for 40+ Indian Brokers

> Place, modify, cancel and track orders across **40+ Indian brokers** from your own **C# / .NET** code. Write your logic once, trade on any broker, and never change a line of code to switch. Part of **[AutoTrader Web](https://stocksdeveloper.in/)** by **Stocks Developer**.

[![NuGet](https://img.shields.io/badge/NuGet-StocksDeveloper.AutoTraderWeb.Api-004880)](https://www.nuget.org/packages/StocksDeveloper.AutoTraderWeb.Api/)
[![Brokers supported](https://img.shields.io/badge/brokers-40%2B-2ea44f)](https://stocksdeveloper.in/#supported-brokers)
[![Free trial](https://img.shields.io/badge/free%20trial-1%20month-blue)](https://webx.stocksdeveloper.in/register)
[![Uptime](https://img.shields.io/badge/uptime-99.98%25-brightgreen)](https://stocksdeveloper.in/features/)
[![API docs](https://img.shields.io/badge/docs-API%20reference-8a2be2)](https://stocksdeveloper.in/documentation/api/)

---

## What is this library?

The **AutoTrader Web C# library** (`StocksDeveloper.AutoTraderWeb.Api`) is a broker-independent trading client for .NET. You write your strategy once in C# and run it against any broker that AutoTrader Web supports. The same function calls work on every broker, so changing broker does not mean rewriting your code.

- **Broker independent.** One codebase works across 40+ Indian brokers. The system handles each broker's API and trading-symbol format for you.
- **Full order control.** Regular, bracket, cover and advanced orders, plus modify, cancel and square-off.
- **Bracket and cover orders on every broker.** AutoTrader's own bracket and cover orders, with a trailing stop-loss, work even on brokers that do not offer them.
- **Read your portfolio.** Live orders, positions, holdings and margins from your code.
- **Multi-account.** Place into one account or many at once.
- **Direct connection.** Your code talks to AutoTrader Web over a secure web connection. Nothing extra to install.

## What is AutoTrader Web?

**[AutoTrader Web](https://stocksdeveloper.in/)** by **Stocks Developer** is copy trading and multi-account software for Indian brokers. Monitor every broker account on one screen and act across all of them at once.

- **All your accounts, one screen.** Live, consolidated P&L, holdings, positions, orders and margins across every account and broker.
- **Copy trading, two ways.** PMS copy from our terminal, and master-child copy in the background, across brokers, with per-account sizing. [Copy trading software](https://stocksdeveloper.in/copy-trading-software/)
- **Bulk orders.** Place, modify, cancel and square-off across many accounts in one action.
- **GTT, bracket and cover orders**, order slicing and market price protection.
- **AutoTrader bracket and cover orders** with a trailing stop-loss, on every broker we support.
- **TradingView automation.** Turn your own chart alerts into real orders.
- **APIs and SDKs.** C#, Java, Python and Excel, plus AmiBroker, MetaTrader and HTTP REST / CSV.
- **8+ years in operation. 99.98% uptime. 40+ brokers. Under 100 ms data latency.**

## Why traders and developers choose us

- 🆓 **Free static IP included** with every account. Saves up to **₹500 per broker account per month** that other tools charge extra for.
- 💸 **One of the lowest prices in the category.** **₹295 to ₹495 per account per month**, all taxes and the static IP included. No setup fee, no hidden charges.
- ☁️ **Nothing to install for the platform.** Monitor and trade from your browser on PC or mobile, from anywhere. (Developer libraries connect directly to our servers.)
- 🔗 **40+ Indian brokers on one platform.** One of the widest broker coverages available.
- 🔁 **Two ways to copy trade**, PMS and master-child, both included.
- 🔒 **Security you control.** API credentials encrypted and stored in India, broker OAuth login and two-factor authentication, portfolio data never stored, plus a Kill Switch and a full activity log.
- 🎁 **Free 1-month trial** on supported brokers.

## Supported brokers

AutoTrader Web works with **40+ Indian brokers**:

5paisa · AC Agarwal · Aetram Trades · Alice Blue · Ambalal Shares · Anand Rathi · Angel One · Arham Share · ATS · AxisDirect · Choice · DBOnline · Dhan · Eureka Share · Finvasia · Flattrade · FYERS · Groww · IIFL Securities · Jainam (Prop & Retail) · Kotak Securities · Mastertrust · Mirae Asset Sharekhan · MLB Stock Broking · Motilal Oswal · Nuvama · PL Capital (PLIndia) · Profitmart · Pune E-Stock Broking (PESB) · Raghunandan Money · Religare · Share India (Prop & Retail) · SMC India · Stocko · SW Capital · Tradejini · Tradeswift · Upstox · Wisdom Capital · Zebu · Zerodha

*Plus any broker that supports the Symphony XTS API.* See the [full, always-current broker list](https://stocksdeveloper.in/#supported-brokers) and the [broker setup guides](https://stocksdeveloper.in/documentation/supported-brokers/).

## Quick start

Install the [`StocksDeveloper.AutoTraderWeb.Api`](https://www.nuget.org/packages/StocksDeveloper.AutoTraderWeb.Api/) package from NuGet.

Create one instance with your API key and read live positions. The same instance places, modifies and cancels orders too, and the same calls work on every supported broker:

```csharp
using com.dakshata.autotrader.api;
using com.dakshata.trading.model.platform;

IAutoTrader autoTrader = AutoTrader.CreateInstance(
    "<your-api-key>",
    AutoTrader.SERVER_URL);

// Read live positions for an account
IOperationResponse<ISet<PlatformPosition>> response =
    autoTrader.ReadPlatformPositions("ABC123");

Console.WriteLine("Message: {0}", response.Message);
foreach (PlatformPosition p in response.Result)
    Console.WriteLine("{0}", p);
```

To place an order instead, call `PlaceRegularOrder` with your account, `"<exchange>"`, a symbol such as `"SBIN"`, the trade type, order type and product type.

Full step-by-step guide: **[C# library setup](https://stocksdeveloper.in/documentation/client-setup/c-library/)**. Get your API key from your [account settings](https://webx.stocksdeveloper.in/register).

## Pricing and free trial

- **Free 1-month trial** on supported brokers, with every feature included.
- Then **₹295 to ₹495 per account per month**. All taxes and a free static IP are included. No setup fee, no hidden charges.
- [See full pricing](https://stocksdeveloper.in/pricing/) · [Start free](https://webx.stocksdeveloper.in/register)

## Documentation and links

| Resource | Link |
|---|---|
| 🌐 Website | https://stocksdeveloper.in/ |
| ✨ Features | https://stocksdeveloper.in/features/ |
| 💰 Pricing | https://stocksdeveloper.in/pricing/ |
| 🔁 Copy trading software | https://stocksdeveloper.in/copy-trading-software/ |
| 🏦 Supported brokers | https://stocksdeveloper.in/#supported-brokers |
| 🔒 Security and data handling | https://stocksdeveloper.in/security/ |
| 📘 Documentation | https://stocksdeveloper.in/documentation/getting-started/ |
| 🧩 API reference | https://stocksdeveloper.in/documentation/api/ |
| ⚙️ C# library setup | https://stocksdeveloper.in/documentation/client-setup/c-library/ |
| 📦 NuGet package | https://www.nuget.org/packages/StocksDeveloper.AutoTraderWeb.Api/ |
| 🆓 Start free (1-month trial) | https://webx.stocksdeveloper.in/register |
| ✉️ Contact us | https://stocksdeveloper.in/contact/ |

## About Stocks Developer

Stocks Developer is a technology company building software tools for Indian markets, shaped by 8+ years of trader feedback. Our software runs on Google Cloud in its Mumbai, India region for fast, low-latency performance, with strong security and high reliability.

Stocks Developer provides software tools only. It gives no investment advice, tips, recommendations, or trading strategies, and it makes no trading decisions for you. All trading and investment decisions remain solely your responsibility. You set up and control every activity, and you can stop it at any time.
