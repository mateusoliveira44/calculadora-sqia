using TechMedicos.Core;
using TechMedicos.Domain.Aggregates;
using TechMedicos.Domain.Enums;

namespace TechMedicos.UnitTests.Domain
{
    [Trait("Domain", "ConsultaTest")]
    public class ConsultaTest
    {
        [Fact]
        public void CriarConsulta_DeveRetornarSucesso()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;

            // Act
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);

            // Assert
            Assert.Equal(medicoId, consulta.MedicoId);
            Assert.Equal(pacienteId, consulta.PacienteId);
            Assert.Equal(dataConsulta, consulta.DataConsulta);
            Assert.Equal(valor, consulta.Valor);
            Assert.Equal(StatusConsulta.Agendada, consulta.Status);
        }

        [Theory]
        [InlineData("", "166DE171-F91F-497D-9933-1CEAA8CC296E", 100)]
        [InlineData("166DE171-F91F-497D-9933-1CEAA8CC296E", "", 100)]
        public void CriarConsulta_Invalida_DeveLancarArgumentException(string medicoId, string pacienteId, decimal valor)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Consulta(medicoId, pacienteId, DateTime.Now, valor));
        }

        [Fact]
        public void AceitarConsulta_DeveRetornarSucesso()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);

            // Act
            consulta.Aceitar();

            // Assert
            Assert.Equal(StatusConsulta.Confirmada, consulta.Status);
        }

        [Fact]
        public void AceitarConsulta_StatusInvalido_DeveLancarException()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);
            consulta.Cancelar("Sem justificativa");

            // Act & Assert 
            Assert.Throws<DomainException>(() => consulta.Aceitar());
        }

        [Fact]
        public void RecusarConsultaComJustificativaPadrao_DeveRetornarSucesso()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);
            string justificativa = null;
            // Act
            consulta.Recusar(justificativa);

            // Assert
            Assert.Equal(StatusConsulta.Rejeitada, consulta.Status);
            Assert.Equal("Necessário reagendar", consulta.Justificativa);
        }

        [Fact]
        public void RecusarConsulta_DeveRetornarSucesso()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);
            string justificativa = "justificativa";
            // Act
            consulta.Recusar(justificativa);

            // Assert
            Assert.Equal(StatusConsulta.Rejeitada, consulta.Status);
            Assert.Equal(justificativa, consulta.Justificativa);
        }

        [Fact]
        public void RecusarConsulta_StatusInvalido_DeveLancarException()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);
            consulta.Cancelar("Sem justificativa");

            // Act & Assert 
            Assert.Throws<DomainException>(() => consulta.Recusar());
        }

        [Fact]
        public void RealizarConsulta_DeveRetornarSucesso()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);
            consulta.Aceitar();

            // Act
            consulta.Realizar();

            // Assert
            Assert.Equal(StatusConsulta.Realizada, consulta.Status);
        }

        [Fact]
        public void RealizarConsulta_StatusInvalido_DeveLancarException()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);

            // Act & Assert 
            Assert.Throws<DomainException>(() => consulta.Realizar());
        }

        [Fact]
        public void CancelarConsulta_DeveRetornarSucesso()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            string justificativa = "Sem justificativa";
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);

            // Act
            consulta.Cancelar(justificativa);

            // Assert
            Assert.Equal(StatusConsulta.Cancelada, consulta.Status);
            Assert.Equal(justificativa, consulta.Justificativa);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void CancelarConsulta_JustificativaVazia_DeveLancarDomainException(string justificativa)
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);

            // Act & Assert
            Assert.Throws<DomainException>(() => consulta.Cancelar(justificativa));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("12")]
        public void CancelarConsulta_JustificativaInvalida_DeveLancarDomainException(string justificativa)
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);

            // Act & Assert
            Assert.Throws<DomainException>(() => consulta.Cancelar(justificativa));
        }

        [Fact]
        public void CancelarConsulta_JaRejeitada_DeveLancarDomainException()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            string justificativa = "Sem justificativa";
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);
            consulta.Recusar();

            // Act & Assert
            Assert.Throws<DomainException>(() => consulta.Cancelar(justificativa));
        }

        [Fact]
        public void CancelarConsulta_JaRealizada_DeveLancarDomainException()
        {
            // Arrange
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            string justificativa = "Sem justificativa";
            var consulta = new Consulta(medicoId, pacienteId, dataConsulta, valor);
            consulta.Aceitar();
            consulta.Realizar();

            // Act & Assert
            Assert.Throws<DomainException>(() => consulta.Cancelar(justificativa));
        }

        [Fact]
        public void AtualizarConsulta_DeveRetornarSucesso()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            string medicoId = Guid.NewGuid().ToString();
            string pacienteId = Guid.NewGuid().ToString();
            DateTime dataConsulta = DateTime.Now;
            decimal valor = 100;
            StatusConsulta status = StatusConsulta.Realizada;

            // Act
            var consulta = new Consulta(id, medicoId, pacienteId, dataConsulta, valor, status);

            // Assert
            Assert.Equal(id, consulta.Id);
            Assert.Equal(medicoId, consulta.MedicoId);
            Assert.Equal(pacienteId, consulta.PacienteId);
            Assert.Equal(dataConsulta, consulta.DataConsulta);
            Assert.Equal(valor, consulta.Valor);
            Assert.Equal(status, consulta.Status);
        }
    }
}
