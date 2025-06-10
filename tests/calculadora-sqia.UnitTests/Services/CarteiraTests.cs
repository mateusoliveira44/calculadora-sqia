using calculadora_sqia.Application.Ports.Repositories;
using calculadora_sqia.Application.Ports.Services;
using calculadora_sqia.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace calculadora_sqia.UnitTests.Services
{
    [Trait("Services", "Carteira")]
    public class CarteiraTests
    {
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly CarteiraService _service;

        public CarteiraTests()
        {
            _cotacaoRepository = Substitute.For<ICotacaoRepository>();
            var logger = Substitute.For<ILogger<CarteiraService>>();
            _service = new CarteiraService(_cotacaoRepository, logger);
        }

        [Fact]
        public async Task DeveCalcularSaldoComSucesso()
        {
            // Arrange
            decimal valor = 1000;
            DateTime dataAplicacao = new DateTime(2025, 6, 9);
            DateTime dataFim = new DateTime(2025, 6, 13);

            var cotacoes = new List<Cotacao>
            {
                new(1, new DateTime(2025, 6, 9), "SQI", 10.00m),
                new(2, new DateTime(2025, 6, 10), "SQI", 10.00m),
                new(3, new DateTime(2025, 6, 11), "SQI", 10.00m),
                new(4, new DateTime(2025, 6, 12), "SQI", 10.00m),
                new(5, new DateTime(2025, 6, 13), "SQI", 10.00m),
            };

            _cotacaoRepository.GetCotacoesAsync(dataAplicacao, dataFim).Returns(Task.FromResult(cotacoes));

            // Act
            var resultado = await _service.ConsultaSaldoPosFixadoAsync(valor, dataAplicacao, dataFim);

            // Assert
            Assert.True(resultado.ValorAtualizado > valor);
            Assert.True(resultado.FatorAcumulado > 1);
        }

        [Fact]
        public async Task DeveLancarExcecao_QuandoDataAplicacaoMaiorQueDataFim()
        {
            // Arrange
            decimal valor = 1000;
            DateTime dataAplicacao = new(2025, 1, 10);
            DateTime dataFim = new(2025, 1, 1);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.ConsultaSaldoPosFixadoAsync(valor, dataAplicacao, dataFim));
        }

        [Fact]
        public async Task DeveLancarExcecao_QuandoNaoExistemCotacoes()
        {
            // Arrange
            decimal valor = 1000;
            DateTime dataAplicacao = new DateTime(2025, 1, 1);
            DateTime dataFim = new DateTime(2025, 1, 10);

            _cotacaoRepository.GetCotacoesAsync(dataAplicacao, dataFim).Returns(Task.FromResult(new List<Cotacao>()));

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.ConsultaSaldoPosFixadoAsync(valor, dataAplicacao, dataFim));
        }
    }
}
