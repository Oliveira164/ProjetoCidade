using Microsoft.AspNetCore.Mvc;

namespace ProjetoCidade.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
