using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Model.Dao
{
    // Classe de conexão com o banco de dados (Material 02)
    internal class ConexaoDB
    {
        // Conexão padrão caso não exista a "CadEtec" no Web.config.
        // Ajuste o Data Source conforme o SQL Server do seu computador.
        private const string StringPadrao =
            "Data Source=.; Initial Catalog=CadEtec; Integrated Security=True; TrustServerCertificate=True";

        private SqlConnection con;

        // Construtor privado: somente a própria classe cria a conexão.
        private ConexaoDB()
        {
            string cs = StringPadrao;
            ConnectionStringSettings cfg = ConfigurationManager.ConnectionStrings["CadEtec"];
            if (cfg != null && !string.IsNullOrWhiteSpace(cfg.ConnectionString))
            {
                cs = cfg.ConnectionString;
            }
            con = new SqlConnection(cs);
        }

        // Retorna uma conexão pronta para uso. Cada chamada devolve uma conexão
        // nova, assim requisições simultâneas na WEB não disputam o mesmo objeto.
        public static ConexaoDB statusDB()
        {
            return new ConexaoDB();
        }

        public SqlConnection getCon()
        {
            return con;
        }

        public void CloseDB()
        {
            if (con != null)
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                con.Dispose();
            }
        }
    }
}
