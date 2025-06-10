using calculadora_sqia.Application.DTOs;
using calculadora_sqia.Application.Ports.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace calculadora_sqia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarteiraController : ControllerBase
    {
        /// <summary>
        /// Consulta o saldo de uma aplicação pós-fixada em uma carteira.
        /// </summary>
        /// <param name="valor">Valor investido na aplicação.</param>
        /// <param name="dataAplicacao">Data de início da aplicação.</param>
        /// <param name="dataFim">Data final da simulação.</param>
        /// <returns>Informações sobre o saldo da aplicação.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(SaldoPosFixadoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConsultaSaldoPosFixado(
            [FromQuery] decimal valor,
            [FromQuery] DateTime dataAplicacao,
            [FromQuery] DateTime dataFim,
            [FromServices] ICarteiraService carteiraService)
        {

            var response = await carteiraService.ConsultaSaldoPosFixadoAsync(valor, dataAplicacao, dataFim);

            return response is not null? 
                Ok(response) : 
                BadRequest("Erro ao consultar saldo da aplicação.");
        }

    }
}
