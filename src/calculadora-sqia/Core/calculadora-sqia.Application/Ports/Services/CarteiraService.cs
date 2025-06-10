using calculadora_sqia.Application.DTOs;
using calculadora_sqia.Application.Ports.Repositories;
using calculadora_sqia.Application.Ports.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace calculadora_sqia.Application.Ports.Services
{
    public class CarteiraService : ICarteiraService
    {
        private const int totalDiasMercado = 252;
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly ILogger<CarteiraService> _logger;

        public CarteiraService(ICotacaoRepository cotacaoRepository, ILogger<CarteiraService> logger)
        {
            _cotacaoRepository = cotacaoRepository;
            _logger = logger;
        }

        public async Task<SaldoPosFixadoResponseDTO > ConsultaSaldoPosFixadoAsync(decimal valor, DateTime dataAplicacao, DateTime dataFim)
        {
            _logger.LogInformation("Iniciando cálculo de saldo pós-fixado. Valor: {valor}, Data Aplicação: {dataAplicacao}, Data Fim: {dataFim}", valor, dataAplicacao, dataFim);

            if (dataAplicacao > dataFim)
            {
                _logger.LogWarning("Data de aplicação {dataAplicacao} maior que data fim {dataFim}", dataAplicacao, dataFim);
                throw new ArgumentException("A data de aplicação não pode ser maior que a data fim.");            
            }            
                
           
            if (valor <= 0)
            {
                _logger.LogWarning("Valor informado é menor ou igual a zero: {valor}", valor);
                    throw new ArgumentException("O valor deve ser maior que zero.");
            }        

            var cotacoes = await _cotacaoRepository.GetCotacoesAsync(dataAplicacao, dataFim);

            if (cotacoes.Count == 0)
            {
                _logger.LogWarning("Nenhuma cotação encontrada entre {dataAplicacao} e {dataFim}", dataAplicacao, dataFim);
                throw new ArgumentException("Não existem cotações cadastradas.");
            }            
                
            

            decimal fatorAcumulado = 1m;
            decimal valAtualizado = valor;
            var diasUteis = GetListaDiasUteis(dataAplicacao, dataFim);

            

            foreach (var dia in diasUteis)
            {
                var cotacao = cotacoes.FirstOrDefault(c => c.Data.Date == DiaUtil(dia));
                if (cotacao == null)
                {
                    _logger.LogError("Cotação não encontrada para o dia útil {dia}", dia);
                    throw new ArgumentException($"Não existe cotação para o dia {dia:dd/MM/yyyy}.");
                }                
                    
                decimal taxa = cotacao.Valor / 100;
                decimal fatorDiario = (decimal)Math.Round(Math.Pow(1 + (double)taxa, 1.0 / totalDiasMercado), 8);

                fatorAcumulado = Truncar(fatorAcumulado * fatorDiario, 16);
                valAtualizado = Truncar(valAtualizado * fatorDiario, 8);
            }

            var resultado = new SaldoPosFixadoResponseDTO { ValorAtualizado = valAtualizado, FatorAcumulado = fatorAcumulado };

            return resultado;
        }

        private List<DateTime> GetListaDiasUteis(DateTime dataAplicacao, DateTime dataFim)
        {
            var listaDiasUteis = new List<DateTime>();

            dataAplicacao = dataAplicacao.AddDays(1);
            for (var dataInicio = dataAplicacao; dataInicio <= dataFim; dataInicio = dataInicio.AddDays(1))
            {
                if (dataInicio.DayOfWeek != DayOfWeek.Saturday && dataInicio.DayOfWeek != DayOfWeek.Sunday)
                {
                    listaDiasUteis.Add(dataInicio);
                }
            }
            _logger.LogInformation("Total de dias úteis: {qtdDias}", listaDiasUteis.Count);

            return listaDiasUteis;
        }

        private DateTime DiaUtil(DateTime dia)
        {
            var diaAnterior = dia.AddDays(-1);
            while (diaAnterior.DayOfWeek == DayOfWeek.Saturday || diaAnterior.DayOfWeek == DayOfWeek.Sunday)
            {
                diaAnterior = diaAnterior.AddDays(-1);
            }
            return diaAnterior;
        }

        private decimal Truncar(decimal valor, int casasDecimais)
        {
            decimal fator = (decimal)Math.Pow(10, casasDecimais);
            return Math.Truncate(valor * fator) / fator;
        }
    }
}
