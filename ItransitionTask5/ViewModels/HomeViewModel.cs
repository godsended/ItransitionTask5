using ItransitionTask5.Models;

namespace ItransitionTask5.ViewModels;

public class HomeViewModel
{
    public string Name { get; set; } = null!;
    public string HostUrl { get; set; } = null!;
    public NamesContext NamesContext { get; set; } = null!;
}