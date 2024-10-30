using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Stocks.Realtime.Api.Stocks;
using System.Globalization;
using System.Text.Json;

namespace StockTracker.API.Stocks;

internal sealed class StocksClient(
    HttpClient httpClient,
    IConfiguration configuration,
    IMemoryCache memoryCache,
    ILogger<StocksClient> logger)
{
    public async Task<StockPriceResponse?> GetDataForTicker(string ticker)
    {
        logger.LogInformation("Getting stock price information for {Ticker}", ticker);

        StockPriceResponse? stockPriceResponse = await memoryCache.GetOrCreateAsync($"stocks-{ticker}", async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            return await GetStockPrice(ticker);
        });

        if (stockPriceResponse is null)
            logger.LogWarning("Failed to get stock price information for {Ticker}", ticker);
        else
        {
            logger.LogInformation(
                "Completed getting stock price information for {Ticker}, {@Stock}",
                ticker,
                stockPriceResponse);
        }

        return stockPriceResponse;
    }

    private async Task<StockPriceResponse?> GetStockPrice(string ticker)
    {
        var httpResponse = await httpClient.GetAsync(
            $"?function=TIME_SERIES_INTRADAY&symbol={ticker}&interval=15min&apikey={configuration["Stocks:ApiKey"]}");

        AlphaVantageData? tickerData = null;

        if (httpResponse.IsSuccessStatusCode)
        {
            string tickerDataString = await httpResponse.Content.ReadAsStringAsync();

            using (JsonDocument document = JsonDocument.Parse(tickerDataString))
            {
                JsonElement rootElement = document.RootElement;

                if (rootElement.TryGetProperty("Error Message", out JsonElement errorMessage))
                    logger.LogError("Unable to find data for {Ticket} {Message}", ticker, errorMessage.ToString());
                else
                    tickerData = JsonConvert.DeserializeObject<AlphaVantageData>(tickerDataString);
            }
        }

        TimeSeriesEntry? lastPrice = tickerData?.TimeSeries.FirstOrDefault().Value ?? null;

        if (lastPrice is null)
            return null;

        return new StockPriceResponse(ticker, decimal.Parse(lastPrice.High, CultureInfo.InvariantCulture));
    }

    public async Task<List<StockSearchResponse>> SearchStocks(string stockName)
    {
        List<StockSearchResponse> response = new();

        string tickerDataString = await httpClient.GetStringAsync(
            $"?function=SYMBOL_SEARCH&keywords={stockName}&apikey={configuration["Stocks:ApiKey"]}");

        AlphaVantageTickerSearchResponse tickerSearch = JsonConvert.DeserializeObject<AlphaVantageTickerSearchResponse>(tickerDataString) ?? new() { bestMatches = new() };

        foreach (var item in tickerSearch.bestMatches)
            response.Add(new StockSearchResponse(item.Synmbol, item.TickerName));

        return response;
    }
}