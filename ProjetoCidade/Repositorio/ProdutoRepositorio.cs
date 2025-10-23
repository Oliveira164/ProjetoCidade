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

        public Produto ObterProduto(string nome)
        {
            // Cria uma nova instância da conexão MySQL dentro de um bloco 'using'.
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL.
                conexao.Open();
                // Cria um novo comando SQL para selecionar todos os campos da tabela 'Produto' onde o campo 'nome' corresponde ao parâmetro fornecido.
                MySqlCommand cmd = new("SELECT * FROM Produto WHERE nome = @nome", conexao);
                // Adiciona um parâmetro ao comando SQL para o campo 'Nome', especificando o tipo como VarChar e utilizando o valor do parâmetro 'nome'.
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;

                // Executa o comando SQL SELECT e obtém um leitor de dados (MySqlDataReader). O CommandBehavior.CloseConnection garante que a conexão
                // será fechada automaticamente quando o leitor for fechado.
                using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    // Inicializa uma variável 'produto' como null. Ela será preenchida se um produto for encontrado.
                    Produto produto = null;
                    // Lê a próxima linha do resultado da consulta. Retorna true se houver uma linha e false caso contrário.
                    if (dr.Read())
                    {
                        // Cria uma nova instância do objeto 'Produto'.
                        produto = new Produto
                        {
                            // Lê o valor da coluna "id" da linha atual do resultado, converte para inteiro e atribui à propriedade 'id' do objeto 'produto'.
                            id = Convert.ToInt32(dr["id"]),
                            // Lê o valor da coluna "nome" da linha atual do resultado, converte para string e atribui à propriedade 'nome' do objeto 'profuto'.
                            nome = dr["nome"].ToString(),
                            // Lê o valor da coluna "descricao" da linha atual do resultado, converte para string e atribui à propriedade 'descricao' do objeto 'produto'.
                            descricao = dr["descricao"].ToString(),
                            // Lê o valor da coluna "preco" da linha atual do resultado, converte para string e atribui à propriedade 'preco' do objeto 'produto'.
                            preco = Convert.ToDecimal(dr["preco"]),
                            // Lê o valor da coluna "quantidade" da linha atual do resultado, converte para string e atribui à propriedade 'quantidade' do objeto 'produto'.
                            quantidade = Convert.ToInt32(dr["quantidade"])
                        };
                    }
                    /* Retorna o objeto 'produto'. Se nenhum usuário foi encontrado com o email fornecido, a variável 'produto'
                     permanecerá null e será retornado.*/
                    return produto;
                }
            }
        }
    }
}
