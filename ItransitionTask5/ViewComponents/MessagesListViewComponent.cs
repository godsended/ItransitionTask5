using Microsoft.AspNetCore.Mvc;

namespace ItransitionTask5.ViewComponents;

[ViewComponent]
public class MessagesListViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}