using ItransitionTask5.Models;

namespace ItransitionTask5.ViewModels;

public class SendMessageViewModel
{
    public string Name { get; set; } = null!;
    public List<NameModel> PossibleNames { get; set; } = null!;
}