using System.Net.WebSockets;

namespace ItransitionTask5.Services;

public interface ISocketDataHandler
{
    Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default);
    Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default);
}