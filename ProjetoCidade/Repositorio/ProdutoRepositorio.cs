using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
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

        // Método para buscar um produto específico pelo seu código (Codigo)
        public Produto ObterProduto(int Codigo)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar um registro da tabela 'cliente' com base no código
                MySqlCommand cmd = new MySqlCommand("SELECT * from Produto where id=@codigo ", conexao);

                // Adiciona um parâmetro para o código a ser buscado, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                // Declara um leitor de dados do MySQL
                MySqlDataReader dr;
                // Cria um novo objeto Cliente para armazenar os resultados
                Produto produto = new Produto();

                /* Executa o comando SQL e retorna um objeto MySqlDataReader para ler os resultados
                CommandBehavior.CloseConnection garante que a conexão seja fechada quando o DataReader for fechado*/

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                // Lê os resultados linha por linha
                while (dr.Read())
                {
                    // Preenche as propriedades do objeto Produto com os valores da linha atual
                    produto.id = Convert.ToInt32(dr["id"]);//propriedade Codigo e convertendo para int
                    produto.nome = (string)(dr["nome"]); // propriedade Nome e passando string
                    produto.descricao = (string)(dr["descricao"]); //propriedade descricao e passando string
                    produto.preco = (decimal)(dr["preco"]); //propriedade preco e passando decimal
                    produto.quantidade = Convert.ToInt32(dr["quantidade"]); //propriedade preco e passando decimal
                }
                // Retorna o objeto Produto encontrado (ou um objeto com valores padrão se não encontrado)
                return produto;
            }
        }

        // Método para listar todos os produtos do banco de dados
        public IEnumerable<Produto> TodosProdutos()
        {
            // Cria uma nova lista para armazenar os objetos Produto
            List<Produto> ProdutoList = new List<Produto>();

            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar todos os registros da tabela 'Produto'
                MySqlCommand cmd = new MySqlCommand("SELECT * from Produto", conexao);

                // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Cria um novo DataTable
                DataTable dt = new DataTable();
                // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                da.Fill(dt);
                // Fecha explicitamente a conexão com o banco de dados 
                conexao.Close();

                // interage sobre cada linha (DataRow) do DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Cria um novo objeto Produto e preenche suas propriedades com os valores da linha atual
                    ProdutoList.Add(
                      new Produto{
                        id = Convert.ToInt32(dr["id"]), // Converte o valor da coluna "id" para inteiro
                        nome = ((string)dr["nome"]), // Converte o valor da coluna "nome" para string
                        descricao = ((string)dr["descricao"]), // Converte o valor da coluna "descricao" para string
                        preco = ((decimal)dr["preco"]), // Converte o valor da coluna "preco" para decimal
                        quantidade = Convert.ToInt32(dr["quantidade"]), // Converte o valor da coluna "quantidade" para inteiro
                      }
                    );
                }
                // Retorna a lista de todos os produtos
                return ProdutoList;
            }
        }

        // Método para Editar (atualizar) os dados de um cliente existente no banco de dados
        public bool Atualizar(Produto produto)
        {
            try
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'produto' com base no código
                    MySqlCommand cmd = new MySqlCommand("Update Produto set nome=@nome, descricao=@descricao, preco=@preco, quantidade=@quantidade" + " where id=@codigo ", conexao);
                    // Adiciona um parâmetro para o código do produto a ser atualizado, definindo seu tipo e valor
                    cmd.Parameters.Add("@codigo", MySqlDbType.Int32).Value = produto.id;
                    // Adiciona um parâmetro para o novo nome, definindo seu tipo e valor
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.nome;
                    // Adiciona um parâmetro para a nova descrição, definindo seu tipo e valor
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.descricao;
                    // Adiciona um parâmetro para o novo preço, definindo seu tipo e valor
                    cmd.Parameters.Add("@preco", MySqlDbType.Decimal).Value = produto.preco;
                    // Adiciona um parâmetro para a nova quantidade, definindo seu tipo e valor
                    cmd.Parameters.Add("@quantidade", MySqlDbType.Int32).Value = produto.preco;
                    // Executa o comando SQL de atualização e retorna o número de linhas afetadas
                    //executa e verifica se a alteração foi realizada
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; // Retorna true se ao menos uma linha foi atualizada

                }
            }
            catch (MySqlException ex)
            {
                // Logar a exceção (usar um framework de logging como NLog ou Serilog)
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return false; // Retorna false em caso de erro

            }
        }

        // Método para excluir um produto do banco de dados pelo seu código (ID)
        public void Excluir(int Id)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();

                // Cria um novo comando SQL para deletar um registro da tabela 'Produto' com base no código
                MySqlCommand cmd = new MySqlCommand("delete from Produto where id=@codigo", conexao);

                // Adiciona um parâmetro para o código a ser excluído, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigo", Id);

                // Executa o comando SQL de exclusão e retorna o número de linhas afetadas
                int i = cmd.ExecuteNonQuery();

                conexao.Close(); // Fecha  a conexão com o banco de dados
            }
        }


    }
}
