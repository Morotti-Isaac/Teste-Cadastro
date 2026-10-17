using Model.Dao;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Neg
{
    // Camada de negócio: validação de campos e duplicidades (Material 07).
    // O campo "estado" do Cliente devolve o código do resultado:
    //   99  = sucesso            1   = erro no banco de dados
    //   20  = nome vazio         2   = nome inválido (1 a 30 caracteres)
    //   60  = endereço vazio     6   = endereço inválido (1 a 50 caracteres)
    //   70  = telefone vazio     7   = telefone inválido (9 a 15 caracteres)
    //   50  = CPF vazio          5   = CPF inválido (11 dígitos)
    //   8   = registro duplicado 33  = registro não encontrado
    //   10  = código não informado   100 = erro inesperado
    public class ClienteNeg
    {
        private ClienteDao objClienteDao;

        public ClienteNeg()
        {
            objClienteDao = new ClienteDao();
        }

        // Valida e normaliza os campos. Retorna true se tudo estiver correto;
        // caso contrário preenche objCliente.estado.
        private bool Validar(Cliente objCliente)
        {
            bool verificacao = true;

            // ----- nome -----
            string nome = objCliente.nome;
            if (nome == null) { objCliente.estado = 20; return false; }
            nome = nome.Trim();
            verificacao = nome.Length <= 30 && nome.Length > 0;
            if (!verificacao) { objCliente.estado = 2; return false; }
            objCliente.nome = nome;

            // ----- endereço -----
            string endereco = objCliente.endereco;
            if (endereco == null) { objCliente.estado = 60; return false; }
            endereco = endereco.Trim();
            verificacao = endereco.Length <= 50 && endereco.Length > 0;
            if (!verificacao) { objCliente.estado = 6; return false; }
            objCliente.endereco = endereco;

            // ----- telefone -----
            string telefone = objCliente.telefone;
            if (telefone == null) { objCliente.estado = 70; return false; }
            telefone = telefone.Trim();
            verificacao = telefone.Length <= 15 && telefone.Length > 8;
            if (!verificacao) { objCliente.estado = 7; return false; }
            objCliente.telefone = telefone;

            // ----- cpf (somente números, 11 dígitos) -----
            string cpf = objCliente.cpf;
            if (cpf == null) { objCliente.estado = 50; return false; }
            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11) { objCliente.estado = 5; return false; }
            objCliente.cpf = cpf;

            return true;
        }

        public void create(Cliente objCliente)
        {
            try
            {
                if (!Validar(objCliente)) return;

                // Verificação de duplicidade (CPF já cadastrado).
                Cliente objClienteAux = new Cliente();
                objClienteAux.cpf = objCliente.cpf;
                if (objClienteDao.find(objClienteAux))
                {
                    objCliente.estado = 8;
                    return;
                }

                objCliente.estado = 99;
                objClienteDao.create(objCliente);   // se falhar, o Dao altera o estado para 1
            }
            catch (Exception)
            {
                objCliente.estado = 100;
            }
        }

        public void update(Cliente objCliente)
        {
            try
            {
                if (objCliente.idCliente <= 0) { objCliente.estado = 10; return; }
                if (!Validar(objCliente)) return;

                // O registro precisa existir.
                Cliente objClienteAux = new Cliente(objCliente.idCliente);
                if (!objClienteDao.find(objClienteAux))
                {
                    objCliente.estado = 33;
                    return;
                }

                // O CPF não pode pertencer a outro cliente.
                Cliente outro = objClienteDao.findByCpf(objCliente.cpf);
                if (outro != null && outro.idCliente != objCliente.idCliente)
                {
                    objCliente.estado = 8;
                    return;
                }

                objCliente.estado = 99;
                objClienteDao.update(objCliente);
            }
            catch (Exception)
            {
                objCliente.estado = 100;
            }
        }

        public void delete(Cliente objCliente)
        {
            try
            {
                // Variável auxiliar repassada ao método find.
                Cliente objClienteAux = new Cliente();
                objClienteAux.idCliente = objCliente.idCliente;

                if (!objClienteDao.find(objClienteAux))
                {
                    objCliente.estado = 33;
                    return;
                }

                objCliente.estado = 99;
                objClienteDao.delete(objCliente);
            }
            catch (Exception)
            {
                objCliente.estado = 100;
            }
        }

        public bool find(Cliente objCliente)
        {
            return objClienteDao.find(objCliente);
        }

        public Cliente findById(long id)
        {
            return objClienteDao.findById(id);
        }

        public List<Cliente> findAll()
        {
            return objClienteDao.findAll();
        }
    }
}
