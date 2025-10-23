//IMPORTA AS BIBLIOTECAS QUE SERÃO UTILIZADAS NO PROJETO
using Microsoft.AspNetCore.Mvc;
using ProjetoCidade.Models;
using ProjetoCidade.Repositorio;
using ProjetoCidade.Models;
using ProjetoCidade.Repositorio;



//DEFINE O NOME E ONDE A CLASSE USURIOCONTROLLER ESTÁ LOCALIZADA
//NAMESPACE AJUDA A ORGANIZAR O CÓDIGO E EVITAR CONFLITOS.
namespace ProjetoCidade.Controllers
{
    //CLASSE USUARIOCONTROLLER QUE ESTA HERDANDO DA CLASSE CONTROLLER
    public class UsuarioController : Controller
    {
        //DECLARA A VARIÁVEL PRIVADA E SOMENTE LEITURA DO TIPO USUARIOREPOSITORIO
        //INSTANCIA O _usuarioController PARA SER ATRIBUIDO NO CONSTRUTOR E INTERAGIR COM OS DADOS
        private readonly UsuarioRepositorio _usuarioRepositorio;

        //DEFINE O CONSTRUTOR DA CLASSE USUARIOCONTROLLER 
        //RECEBE A INSTANCIA DE USUARIOREPOSITORIO COM PARÂMETROS
        public UsuarioController(UsuarioRepositorio usuarioRepositorio)
        {
            //O CONSTRUTOR É CHAMADO QUANDO UMA NOVA INSTANCIA É CRIADA
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpGet]
        //INTERFACE É UMA REPRESENTAÇAO DO RESULTADO (TELA)
        public IActionResult Login()
        {
            //RETORNA A PAGINA INDEX
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _usuarioRepositorio.ObterUsuario(email);

            if (usuario != null && usuario.senha == senha)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Email / Senha Inválidos");


            //RETORNA A PAGINA INDEX
            return View();
        }

        //CADASTRO DO USUÁRIO

        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _usuarioRepositorio.AdicionarUsuario(usuario);
                return RedirectToAction("Login");
            }
            return View(usuario);
        }
    }
}