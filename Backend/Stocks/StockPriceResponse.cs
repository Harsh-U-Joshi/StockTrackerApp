namespace StockTracker.API.Stocks;

public sealed record StockPriceResponse(string Ticker, decimal Price);
public sealed record StockSearchResponse(string stockId, string stockName);


