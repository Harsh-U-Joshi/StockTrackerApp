namespace StockTracker.API.Realtime;

public interface IStockUpdateClient
{
    Task ReceiveStockPriceUpdate(StockPriceUpdate update);
}
