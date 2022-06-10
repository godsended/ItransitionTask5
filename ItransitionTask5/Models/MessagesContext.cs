using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTask5.Models;

public class MessagesContext : DbContext
{
    public DbSet<Message> Messages { get; set; }

    public MessagesContext(DbContextOptions<MessagesContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public Message InitMessage(Message message)
    {
        message.DateTime = DateTime.Now.ToString();
        return message;
    }

    public List<Message> GetMessagesWithName(string name)
    {
        Debug.WriteLine("Start search messages with " + name);
        List<Message> messagesList = Messages.Where(message => message.From == name || message.To == name).ToList();
        Debug.WriteLine("Found " + messagesList.Count() + " messages for " + name);
        return messagesList;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=task5;Username=postgres;Password=174285396020");
    }
}