using System.ComponentModel.DataAnnotations;
using ItransitionTask5.Tools;

namespace ItransitionTask5.Models;

[Serializable]
public class Message
{
    [Key] public int Id { get; set; }

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;

    public int? ReplyFor { get; set; }

    public string Title { get; set; } = null!;

    public string Text { get; set; } = null!;

    public string DateTime { get; set; } = null!;

    public static bool Validate(Message? message)
    {
        return message != null && !StringTools.StringEmpty(
            message.From, message.To, message.Text, message.Title);
    }
}