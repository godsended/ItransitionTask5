using Microsoft.AspNetCore.Mvc;

namespace ItransitionTask5.ViewComponents;

[ViewComponent]
public class MessageViewViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}