using calculadora_sqia.Application.DTOs;

namespace calculadora_sqia.Application.Ports.Services.Interfaces
{
    public interface ICarteiraService
    {
        Task<SaldoPosFixadoResponseDTO> ConsultaSaldoPosFixadoAsync(decimal valor, DateTime dataAplicacao, DateTime dataFim);
    }
}
