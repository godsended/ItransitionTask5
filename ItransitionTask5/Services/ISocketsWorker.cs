using System.Net.WebSockets;
using ItransitionTask5.Models;

namespace ItransitionTask5.Services;

public interface ISocketsWorker
{
    void NotifySocket(string name, Message message);
    void AddSocket(string name, WebSocket socket);
}