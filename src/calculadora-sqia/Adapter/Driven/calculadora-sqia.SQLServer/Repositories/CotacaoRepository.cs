using calculadora_sqia.Application.Ports.Repositories;
using calculadora_sqia.Application.Ports.Repositories.Interfaces;
using calculadora_sqia.Domain.Entities;
using Dapper;

namespace calculadora_sqia.SQLServer.Repositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CotacaoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Cotacao>> GetCotacoesAsync(DateTime dataAplicacao, DateTime dataFim)
        {
            using var connection = _connectionFactory.CreateConnection();
            var cotacoes = await connection.QueryAsync<Cotacao>(
                @"Select 
	                [Id] = C.id,
	                [Data] = C.[data],
	                [Indexador] = C.indexador,
	                [Valor] = C.valor
                From Cotacao C
                Where C.[data] Between @dataAplicacao and @dataFim
                Order By 2 Asc",
                new { dataAplicacao, dataFim });

            return cotacoes.ToList();
        }
    }
}
