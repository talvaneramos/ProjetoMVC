using Dapper;
using Projeto.Infra.Data.Contracts;
using Projeto.Infra.Data.Dtos;
using Projeto.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Infra.Data.Repositories
{
    public class CompromissoRepository : ICompromissoRepository
    {
        private readonly string connectionString;

        public CompromissoRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Create(Compromisso entity)
        {
            var query = "INSERT INTO Compromisso(Titulo, Descricao, DataInicio, DataFim, HoraInicio, HoraFim, IdUsuario, Categoria)  "
                       + "values(@Titulo, @Descricao, @DataInicio, @DataFim, @HoraInicio, @HoraFim, @IdUsuario, @Categoria) ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Update(Compromisso entity)
        {
            var query = "UPDATE Compromisso SET Titulo = @Titulo, Descricao = @Descricao, DataInicio = @DataInicio, "
                      + "DataFim = @DataFim, HoraInicio = @HoraInicio, HoraFim = @HoraFim, Categoria = @Categoria "
                      + "WHERE IdCompromisso = @IdCompromisso ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Delete(Compromisso entity)
        {
            var query = "DELETE FROM Compromisso WHERE IdCompromisso = @IdCompromisso ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public List<Compromisso> GetAll()
        {
            var query = "SELECT * from Compromisso order by DataInicio desc";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Compromisso>(query).ToList();
            }
        }

        public Compromisso GetById(int id)
        {
            var query = "SELECT * from Compromisso WHERE IdCompromisso = @IdCompromisso ";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<Compromisso>(query, new { IdCompromisso = id });
            }
        }

        public List<Compromisso> GetByDatas(DateTime dataMin, DateTime dataMax, int idUsuario)
        {
            var query = "Select * from Compromisso WHERE IdUsuario = @IdUsuario and DataInicio between @DataMin and @DataMax ";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Compromisso>(query, new { DataMin = dataMin, DataMax = dataMax, IdUsuario = idUsuario }).ToList();
            }
        }

        public List<ResumoCategoriaDto> GetResumoCategoria(int idUsuario)
        {
            var query = "SELECT Categoria, count(*) as Quantidade from Compromisso "
                      + "WHERE IdUsuario = @IdUsuario group by Categoria";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<ResumoCategoriaDto>
                    (query, new { IdUsuario =  idUsuario }).ToList();
            }
        }
    }
}
