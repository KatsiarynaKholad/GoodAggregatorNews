using Microsoft.AspNetCore.Mvc;

namespace GoodAggregatorNews.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
