using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ItransitionTask5.Models;
using ItransitionTask5.Services;
using ItransitionTask5.Tools;

namespace ItransitionTask5.Middlewares;

public class SocketDataHandlerMiddleware
{
    private readonly RequestDelegate next;

    public SocketDataHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, ISocketDataHandler dataHandler, MessagesContext messagesContext,
        ISocketsWorker socketsWorker, NamesContext namesContext)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            do
            {
                await Echo(webSocket, dataHandler, messagesContext, socketsWorker, namesContext);
            } while (webSocket.CloseStatus == null);
        }
        else
        {
            await next(context);
        }
    }

    private async Task Echo(WebSocket webSocket, ISocketDataHandler dataHandler, MessagesContext messagesContext,
        ISocketsWorker socketsWorker, NamesContext namesContext)
    {
        string request = await dataHandler.ReceiveStringAsync(webSocket);
        MessageRequest? messageRequest;
        try
        {
            messageRequest = JsonSerializer.Deserialize<MessageRequest>(request);
            foreach (var recipient in messageRequest.To)
            {
                Message message = new Message
                {
                    From = messageRequest.From,
                    To = recipient,
                    Title = messageRequest.Title,
                    Text = messageRequest.Text
                };
                await HandleMessage(messagesContext, socketsWorker, message!);
            }
            var answer = new {IsSuccess = true};
            await dataHandler.SendStringAsync(webSocket, JsonSerializer.Serialize(answer));
        }
        catch (JsonException exception)
        {
            HandleNameRequest(request, webSocket, socketsWorker, messagesContext, namesContext);
            Debug.WriteLine("Its not message: " + request);
        }
    }

    private async Task HandleMessage(MessagesContext messagesContext, ISocketsWorker socketsWorker, Message message)
    {
        if (Message.Validate(message))
        {
            messagesContext.Messages.Add(messagesContext.InitMessage(message));
            if (message.From != message.To)
                socketsWorker.NotifySocket(message.To, message);
            await messagesContext.SaveChangesAsync();
        }
    }

    private bool HandleNameRequest(string request, WebSocket socket, ISocketsWorker socketsWorker,
        MessagesContext messagesContext, NamesContext namesContext)
    {
        if (!string.IsNullOrWhiteSpace(request))
        {
            string name = request.Substring(1, request.Length - 1)
                .Substring(0, request.Length - 2);
            socketsWorker.AddSocket(name, socket);
            namesContext.TryAddName(name);
            SendMessagesOnNameRequest(name, messagesContext, socketsWorker);
            return true;
        }

        return false;
    }

    private void SendMessagesOnNameRequest(string name, MessagesContext messagesContext, ISocketsWorker socketsWorker)
    {
        Debug.WriteLine("Start sending init messages: ");
        List<Message> messages = messagesContext.GetMessagesWithName(name);
        foreach (var message in messages)
        {
            Debug.WriteLine("Send message for " + name);
            socketsWorker.NotifySocket(name, message);
        }
    }

    [Serializable]
    private class MessageRequest
    {
        public string From { get; set; } = null!;

        public string[] To { get; set; } = null!;

        public int? ReplyFor { get; set; }

        public string Title { get; set; } = null!;

        public string Text { get; set; } = null!;
    }
}

public static class SocketDataHandlerExtensions
{
    public static IApplicationBuilder UseSocketDataHandler(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SocketDataHandlerMiddleware>();
    }
}