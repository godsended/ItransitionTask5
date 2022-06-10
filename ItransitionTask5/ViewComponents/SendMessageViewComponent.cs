using ItransitionTask5.Models;
using ItransitionTask5.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTask5.ViewComponents;

[ViewComponent]
public class SendMessageViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(NamesContext namesContext)
    {
        return View(new SendMessageViewModel() {Name = Request.Cookies["Name"]!, 
            PossibleNames = namesContext.Names.ToList()});
    }
}