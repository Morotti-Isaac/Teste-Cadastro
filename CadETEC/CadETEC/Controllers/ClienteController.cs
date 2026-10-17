using Model.Entity;
using Model.Neg;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CadETEC.Controllers
{
    public class ClienteController : Controller
    {
        // Objeto para fazer relação direta com a camada de negócio (Material 08)
        ClienteNeg objClienteNeg;

        public ClienteController()
        {
            objClienteNeg = new ClienteNeg();
        }

        // Converte o código "estado" devolvido pela camada de negócio em mensagem.
        private string Mensagem(int estado)
        {
            switch (estado)
            {
                case 1:   return "Não foi possível acessar o banco de dados. Verifique a string de conexão e se o SQL Server está em execução.";
                case 2:   return "Nome inválido: informe de 1 a 30 caracteres.";
                case 20:  return "O nome do cliente é obrigatório.";
                case 5:   return "CPF inválido: informe os 11 dígitos.";
                case 50:  return "O CPF é obrigatório.";
                case 6:   return "Endereço inválido: informe de 1 a 50 caracteres.";
                case 60:  return "O endereço é obrigatório.";
                case 7:   return "Telefone inválido: informe de 9 a 15 caracteres.";
                case 70:  return "O telefone é obrigatório.";
                case 8:   return "Já existe um cliente cadastrado com este CPF.";
                case 10:  return "Código do cliente não informado.";
                case 33:  return "Cliente não encontrado.";
                case 100: return "Ocorreu um erro inesperado. Tente novamente.";
                default:  return "Não foi possível concluir a operação.";
            }
        }

        // GET: Cliente  -> mostra todos os clientes
        public ActionResult Index()
        {
            // Lista com acesso ao método findAll
            List<Cliente> Lista = objClienteNeg.findAll();

            // Retorno para a view os dados da lista carregada com os registros.
            return View(Lista);
        }

        // GET: Cliente/Find/5  -> detalhes
        public ActionResult Find(long id = 0)
        {
            Cliente objCliente = objClienteNeg.findById(id);
            if (objCliente == null)
            {
                TempData["Erro"] = Mensagem(33);
                return RedirectToAction("Index");
            }
            return View(objCliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View(new Cliente());
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente objCliente)
        {
            if (!ModelState.IsValid)
            {
                return View(objCliente);
            }

            objClienteNeg.create(objCliente);

            if (objCliente.estado == 99)
            {
                TempData["Sucesso"] = "Cliente cadastrado com sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.Erro = Mensagem(objCliente.estado);
            return View(objCliente);
        }

        // GET: Cliente/Update/5
        public ActionResult Update(long id = 0)
        {
            Cliente objCliente = objClienteNeg.findById(id);
            if (objCliente == null)
            {
                TempData["Erro"] = Mensagem(33);
                return RedirectToAction("Index");
            }
            return View(objCliente);
        }

        // POST: Cliente/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Cliente objCliente)
        {
            if (!ModelState.IsValid)
            {
                return View(objCliente);
            }

            objClienteNeg.update(objCliente);

            if (objCliente.estado == 99)
            {
                TempData["Sucesso"] = "Cliente alterado com sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.Erro = Mensagem(objCliente.estado);
            return View(objCliente);
        }

        // GET: Cliente/Eliminar/5  -> tela de confirmação
        public ActionResult Eliminar(long id = 0)
        {
            Cliente objCliente = objClienteNeg.findById(id);
            if (objCliente == null)
            {
                TempData["Erro"] = Mensagem(33);
                return RedirectToAction("Index");
            }
            return View(objCliente);
        }

        // POST: Cliente/Eliminar/5
        [HttpPost]
        [ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarConfirmado(long id = 0)
        {
            Cliente objCliente = new Cliente(id);
            objClienteNeg.delete(objCliente);

            if (objCliente.estado == 99)
                TempData["Sucesso"] = "Cliente excluído com sucesso!";
            else
                TempData["Erro"] = Mensagem(objCliente.estado);

            return RedirectToAction("Index");
        }
    }
}
