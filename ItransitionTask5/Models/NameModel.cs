using System.ComponentModel.DataAnnotations;

namespace ItransitionTask5.Models;

[Serializable]
public class NameModel
{
    [Key] public int Id { get; set; }

    public string Name { get; set; } = null!;
}