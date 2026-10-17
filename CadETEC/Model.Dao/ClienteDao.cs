using Model.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Model.Dao
{
    // Responsável pela comunicação com a tabela cliente (Material 06)
    public class ClienteDao : Intermediario<Cliente>
    {
        private ConexaoDB objConexaoDb;
        private SqlCommand comando;

        public ClienteDao()
        {
            objConexaoDb = ConexaoDB.statusDB();
        }

        // ---------- métodos auxiliares ----------
        private static void AddCpf(SqlCommand cmd, string cpf)
        {
            decimal valor;
            if (!decimal.TryParse(cpf, out valor)) valor = 0;
            SqlParameter p = cmd.Parameters.Add("@cpf", SqlDbType.Decimal);
            p.Precision = 11;
            p.Scale = 0;
            p.Value = valor;
        }

        private static Cliente Ler(SqlDataReader r)
        {
            return new Cliente(
                Convert.ToInt64(r["idCliente"]),
                r["nome"].ToString(),
                r["endereco"].ToString(),
                r["telefone"].ToString(),
                Convert.ToDecimal(r["cpf"]).ToString("00000000000"));
        }

        // ---------- create ----------
        public void create(Cliente objCliente)
        {
            objConexaoDb = ConexaoDB.statusDB();
            string create = "insert into cliente (nome, endereco, telefone, cpf) " +
                            "values (@nome, @endereco, @telefone, @cpf)";
            try
            {
                comando = new SqlCommand(create, objConexaoDb.getCon());
                comando.Parameters.AddWithValue("@nome", objCliente.nome);
                comando.Parameters.AddWithValue("@endereco", objCliente.endereco);
                comando.Parameters.AddWithValue("@telefone", objCliente.telefone);
                AddCpf(comando, objCliente.cpf);
                objConexaoDb.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                objCliente.estado = 1;
            }
            finally
            {
                objConexaoDb.getCon().Close();
                objConexaoDb.CloseDB();
            }
        }

        // ---------- delete ----------
        public void delete(Cliente objCliente)
        {
            objConexaoDb = ConexaoDB.statusDB();
            string delete = "delete from cliente where idCliente = @id";
            try
            {
                comando = new SqlCommand(delete, objConexaoDb.getCon());
                comando.Parameters.AddWithValue("@id", objCliente.idCliente);
                objConexaoDb.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                objCliente.estado = 1;
            }
            finally
            {
                objConexaoDb.getCon().Close();
                objConexaoDb.CloseDB();
            }
        }

        // ---------- update ----------
        public void update(Cliente objCliente)
        {
            objConexaoDb = ConexaoDB.statusDB();
            string update = "update cliente set nome = @nome, endereco = @endereco, " +
                            "telefone = @telefone, cpf = @cpf where idCliente = @id";
            try
            {
                comando = new SqlCommand(update, objConexaoDb.getCon());
                comando.Parameters.AddWithValue("@nome", objCliente.nome);
                comando.Parameters.AddWithValue("@endereco", objCliente.endereco);
                comando.Parameters.AddWithValue("@telefone", objCliente.telefone);
                AddCpf(comando, objCliente.cpf);
                comando.Parameters.AddWithValue("@id", objCliente.idCliente);
                objConexaoDb.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                objCliente.estado = 1;
            }
            finally
            {
                objConexaoDb.getCon().Close();
                objConexaoDb.CloseDB();
            }
        }

        // ---------- find ----------
        // Retorna true se o cliente existe. Pesquisa pelo idCliente (quando
        // informado) ou, caso contrário, pelo CPF.
        public bool find(Cliente objCliente)
        {
            objConexaoDb = ConexaoDB.statusDB();
            bool achou = false;
            try
            {
                comando = new SqlCommand();
                comando.Connection = objConexaoDb.getCon();
                if (objCliente.idCliente > 0)
                {
                    comando.CommandText = "select count(*) from cliente where idCliente = @id";
                    comando.Parameters.AddWithValue("@id", objCliente.idCliente);
                }
                else
                {
                    comando.CommandText = "select count(*) from cliente where cpf = @cpf";
                    AddCpf(comando, objCliente.cpf);
                }
                objConexaoDb.getCon().Open();
                achou = Convert.ToInt32(comando.ExecuteScalar()) > 0;
            }
            catch (Exception)
            {
                objCliente.estado = 1;
                achou = false;
            }
            finally
            {
                objConexaoDb.getCon().Close();
                objConexaoDb.CloseDB();
            }
            return achou;
        }

        // Retorna o cliente (ou null se não existir) pelo código.
        public Cliente findById(long id)
        {
            return BuscarUm("select idCliente, nome, endereco, telefone, cpf from cliente where idCliente = @id",
                            delegate (SqlCommand c) { c.Parameters.AddWithValue("@id", id); });
        }

        // Retorna o cliente (ou null se não existir) pelo CPF.
        public Cliente findByCpf(string cpf)
        {
            return BuscarUm("select idCliente, nome, endereco, telefone, cpf from cliente where cpf = @cpf",
                            delegate (SqlCommand c) { AddCpf(c, cpf); });
        }

        private Cliente BuscarUm(string sql, Action<SqlCommand> parametros)
        {
            objConexaoDb = ConexaoDB.statusDB();
            Cliente achado = null;
            try
            {
                comando = new SqlCommand(sql, objConexaoDb.getCon());
                parametros(comando);
                objConexaoDb.getCon().Open();
                using (SqlDataReader r = comando.ExecuteReader())
                {
                    if (r.Read()) achado = Ler(r);
                }
            }
            catch (Exception)
            {
                achado = null;
            }
            finally
            {
                objConexaoDb.getCon().Close();
                objConexaoDb.CloseDB();
            }
            return achado;
        }

        // ---------- findAll ----------
        public List<Cliente> findAll()
        {
            objConexaoDb = ConexaoDB.statusDB();
            List<Cliente> lista = new List<Cliente>();
            try
            {
                comando = new SqlCommand(
                    "select idCliente, nome, endereco, telefone, cpf from cliente order by nome",
                    objConexaoDb.getCon());
                objConexaoDb.getCon().Open();
                using (SqlDataReader r = comando.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(Ler(r));
                    }
                }
            }
            catch (Exception)
            {
                // Em caso de erro devolve a lista vazia.
            }
            finally
            {
                objConexaoDb.getCon().Close();
                objConexaoDb.CloseDB();
            }
            return lista;
        }
    }
}
