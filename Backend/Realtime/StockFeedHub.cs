using Microsoft.AspNetCore.SignalR;

namespace StockTracker.API.Realtime;

internal sealed class StockFeedHub : Hub<IStockUpdateClient>
{
    public async Task JoinStockGroup(string ticker)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, ticker);
    }
}
