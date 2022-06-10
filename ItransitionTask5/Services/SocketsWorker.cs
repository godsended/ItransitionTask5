using System.Diagnostics;
using System.Net.WebSockets;
using System.Text.Json;
using ItransitionTask5.Models;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTask5.Services;

public class SocketsWorker : ISocketsWorker
{
    private readonly ISocketDataHandler dataHandler;

    private int nextId = 0;

    private int NextId
    {
        get { return nextId++; }
    }

    private Dictionary<int, KeyValuePair<string, WebSocket>> SocketsByName { get; set; }
        = new Dictionary<int, KeyValuePair<string, WebSocket>>();

    public SocketsWorker(ISocketDataHandler dataHandler)
    {
        this.dataHandler = dataHandler;
    }

    public void NotifySocket(string name, Message message)
    {
        foreach (var socket in FindSocketsByName(name))
        {
            dataHandler.SendStringAsync(socket, JsonSerializer.Serialize(message));
        }
    }

    private List<WebSocket> FindSocketsByName(string name)
    {
        var result = from pair in SocketsByName where pair.Value.Key == name select pair;
        List<WebSocket> sockets = new List<WebSocket>();
        foreach (var pair in result)
        {
            if(!CheckSocketForCloseStatus(pair))
                sockets.Add(pair.Value.Value);
        }
        
        return sockets;
    }

    private bool CheckSocketForCloseStatus(KeyValuePair<int, KeyValuePair<string, WebSocket>> pair)
    {
        if (pair.Value.Value.State is WebSocketState.Aborted or WebSocketState.Closed)
            SocketsByName.Remove(pair.Key);
        return pair.Value.Value.State is WebSocketState.Aborted or WebSocketState.Closed;
    }
    public void AddSocket(string name, WebSocket socket)
    {
        Debug.WriteLine("Add socket for " + name);
        SocketsByName.Add(NextId, new KeyValuePair<string, WebSocket>(name, socket));
    }
}