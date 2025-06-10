using calculadora_sqia.Domain.Entities;

namespace calculadora_sqia.Application.Ports.Repositories
{
    public interface ICotacaoRepository
    {
        Task<List<Cotacao>> GetCotacoesAsync(DateTime dataAplicacao, DateTime dataFim);
    }
}
