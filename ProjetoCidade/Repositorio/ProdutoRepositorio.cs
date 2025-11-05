using MySql.Data.MySqlClient;
using ProjetoCidade.Models;
using System.Data;

namespace ProjetoCidade.Repositorio
{
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        // Declara um campo privado somente leitura para armazenar a string de conexão com o MySQL.
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");

        //METODO CADASTRAR PRODUTO
        public void AdicionarProduto(Produto produto)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                var cmd = conexao.CreateCommand();
                // Cria um novo comando SQL para inserir dados na tabela 'cliente'
                cmd.CommandText = "INSERT INTO Produto (nome,descricao,preco,quantidade) VALUES (@nome, @descricao, @preco, @quantidade)";

                // Adiciona um parâmetro para o nome, definindo seu tipo e valor
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.nome;
                // Adiciona um parâmetro para a descrição, definindo seu tipo e valor
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.descricao;
                // Adiciona um parâmetro para o preço, definindo seu tipo e valor
                cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.preco;
                // Adiciona um parâmetro para a quantidade, definindo seu tipo e valor
                cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.quantidade;

                // Executa o comando SQL de inserção e retorna o número de linhas afetadas
                cmd.ExecuteNonQuery();
                // Fecha explicitamente a conexão com o banco de dados 
                conexao.Close();

            }
        }


        //Método buscar todos os produtos
        public List<Produto> ObterProduto()
        {
            var produtos = new List<Produto>();

            // Cria uma nova instância da conexão MySQL dentro de um bloco 'using'.
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                using (var connection = new MySqlConnection(_conexaoMySQL))
                {
                    connection.Open();
                    var query = "SELECT * FROM Produto";
                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var produto = new Produto
                            {
                                id = reader.GetInt32("id"),
                                nome = reader.GetString("nome"),
                                descricao = reader.GetString("descricao"),
                                preco = reader.GetDecimal("preco"),
                                quantidade = reader.GetInt32("quantidade")
                            };
                            produtos.Add(produto);
                        }
                    }
                }
                return produtos;
            }
        }
    }
}
