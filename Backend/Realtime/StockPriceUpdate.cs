namespace StockTracker.API.Realtime;

public sealed record StockPriceUpdate(string ticker, decimal price);