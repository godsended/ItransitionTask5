using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using ItransitionTask5.Models;

namespace ItransitionTask5.Services;

public class SocketDataHandler : ISocketDataHandler
{
    public Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default)
    {
        var buffer = Encoding.UTF8.GetBytes(data);
        var segment = new ArraySegment<byte>(buffer);
        return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
    }

    public async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default)
    {
        var buffer = new ArraySegment<byte>(new byte[8192]);
        using (var ms = new MemoryStream())
        {
            WebSocketReceiveResult result;
            do
            {
                ct.ThrowIfCancellationRequested();

                result = await socket.ReceiveAsync(buffer, ct);
                ms.Write(buffer.Array, buffer.Offset, result.Count);
            }
            while (!result.EndOfMessage);

            ms.Seek(0, SeekOrigin.Begin);
            if (result.MessageType != WebSocketMessageType.Text)
                throw new Exception("Unexpected message");

            if (result.CloseStatus != null)
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            
            using (var reader = new StreamReader(ms, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}