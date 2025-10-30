using Microsoft.AspNetCore.Mvc;
using ProjetoCidade.Repositorio;
using ProjetoCidade.Models;

namespace ProjetoCidade.Controllers
{
    public class ProdutoController : Controller
    {

        private readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto produto)
        {
            if (ModelState.IsValid)
            {
                _produtoRepositorio.AdicionarProduto(produto);
                return RedirectToAction("Index", "Produto");
            }
            return View(produto);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
