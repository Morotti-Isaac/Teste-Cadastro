using System.ComponentModel.DataAnnotations;

namespace Model.Entity
{
    // Entidade respectiva a tabela Cliente (Material 05)
    public class Cliente
    {
        [Display(Name = "Código")]
        public long idCliente { get; set; }

        [Display(Name = "Nome Cliente")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string nome { get; set; }

        [Display(Name = "Endereço")]
        public string endereco { get; set; }

        [Display(Name = "Telefone")]
        public string telefone { get; set; }

        [Display(Name = "Cpf")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string cpf { get; set; }

        // Objeto estado para controlar o status do cliente.
        // Campo não existente na tabela, uso interno na aplicação.
        public int estado { get; set; }

        // CPF no formato 000.000.000-00 (somente leitura, usado nas Views)
        public string cpfFormatado
        {
            get
            {
                if (cpf != null && cpf.Length == 11)
                {
                    return cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." +
                           cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);
                }
                return cpf;
            }
        }

        public Cliente()
        {
        }

        public Cliente(long idCliente)
        {
            this.idCliente = idCliente;
        }

        public Cliente(long idCliente, string nome, string endereco, string telefone, string cpf)
        {
            this.idCliente = idCliente;
            this.nome = nome;
            this.endereco = endereco;
            this.telefone = telefone;
            this.cpf = cpf;
        }
    }
}
