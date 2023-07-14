using Dapper;
using Projeto.Infra.Data.Contracts;
using Projeto.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string connectionString;

        public UsuarioRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Create(Usuario entity)
        {
            var query = "INSERT INTO Usuario(Nome, Email, Senha, DataCriacao) "
                      + "values(@Nome, @Email, @Senha, @DataCriacao) ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Update(Usuario entity)
        {
            var query = "Update Usuario set Nome = @Nome, Email = @Email, Senha = @Senha "
                      + "WHERE IdUsuario = @IdUsuario ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Delete(Usuario entity)
        {
            var query = "DELETE from Usuario WHERE IdUsuario = @IdUsuario ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }

        }

        public List<Usuario> GetAll()
        {
            var query = "SELECT * from Usuario ";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Usuario>(query).ToList();
            }
        }

        public Usuario GetById(int id)
        {
            var query = "SELECT * from Usuario WHERE IdUsuario = @IdUsuario ";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { IdUsuario = id });
            }
        }
        public Usuario GetByEmail(string email)
        {
            var query = "SELECT * from Usuario WHERE Email = @Email ";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { Email = email });
            }
        }

        public Usuario GetByEmailAndSenha(string email, string senha)
        {
            var query = "SELECT * from Usuario WHERE Email = @Email and Senha = @Senha ";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Usuario>(query, new { Email = email, Senha = senha });
            }
        }
    }
}
