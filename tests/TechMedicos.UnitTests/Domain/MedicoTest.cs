using TechMedicos.Core;
using TechMedicos.Domain.Aggregates;
using TechMedicos.Domain.ValueObjects;

namespace TechMedicos.UnitTests.Domain
{
    [Trait("Domain", "MedicoTest")]
    public class MedicoTest
    {
        [Fact]
        public void CriarMedico_DeveRetornarSucesso()
        {
            // Arrange
            string nome = "Medico Teste";
            string crm = "0000/SP";
            decimal valorConsulta = 100;

            // Act
            var medico = new Medico(nome, crm, valorConsulta);

            // Assert
            Assert.Equal(nome, medico.Nome);
            Assert.Equal(crm, medico.Crm.Documento);
            Assert.Equal(valorConsulta, medico.ValorConsulta);
        }

        [Theory]
        [InlineData("", "0000/SP", 100)]
        public void CriarMedico_Invalido_DeveLancarArgumentException(string nome, string crm, decimal valorConsulta)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new Medico(nome, crm, valorConsulta));
        }

        [Fact]
        public void CriarMedico_Invalido_DeveLancarDomainException()
        {
            // Arrange
            string nome = "a";
            string crm = "0000/SP";
            decimal valorConsulta = 100;

            // Act & Assert
            Assert.Throws<DomainException>(() => new Medico(nome, crm, valorConsulta));
        }

        [Theory]
        [InlineData("0000/OS")]
        [InlineData("000/SP")]
        [InlineData("/SP")]
        [InlineData("00000000000/SP")]
        [InlineData("00000000000")]
        [InlineData("000000-SP")]
        public void CriarMedico_CrmInvalido_DeveLancarException(string crm)
        {
            // Arrange, Act & Assert
            Assert.Throws<DomainException>(() => new Medico("Medico Teste 2", crm, 100));
        }

        [Fact]
        public void AtualizarMedico_DeveRetornarSucesso()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            string nome = "Medico Teste";
            string crm = "0000/SP";
            decimal valorConsulta = 100;

            // Act
            var medico = new Medico(id, nome, crm, valorConsulta, new List<AgendaMedica>());

            // Assert
            Assert.Equal(id, medico.Id);
            Assert.Equal(nome, medico.Nome);
            Assert.Equal(crm, medico.Crm.Documento);
            Assert.Equal(valorConsulta, medico.ValorConsulta);
        }

        [Fact]
        public void AdicionarAgendamento_DeveRetornarSucesso()
        {
            // Arrange
            string nome = "Medico Teste";
            string crm = "0000/SP";
            decimal valorConsulta = 100;
            DateOnly dataAgendamento = new DateOnly(2024, 07, 21);
            var medico = new Medico(nome, crm, valorConsulta);
            var agendamentos = new AgendaMedica(dataAgendamento, new List<HorarioDisponivel>
                {
                    new HorarioDisponivel(new TimeOnly(8, 30))
                });



            // Act
            medico.AdicionarAgendamento(agendamentos);

            // Assert
            Assert.Equal(agendamentos.Data, medico.Agendas.First().Data);
            Assert.Equal(agendamentos.Horarios.First().HoraInicio, medico.Agendas.First().Horarios.First().HoraInicio);
            Assert.Equal(agendamentos.Horarios.First().HoraInicio.AddMinutes(50), medico.Agendas.First().Horarios.First().HoraFim);
        }

        [Fact]
        public void AdicionarAgendamento_DataJaExistente_DeveLancarException()
        {
            // Arrange
            string nome = "Medico Teste";
            string crm = "0000/SP";
            decimal valorConsulta = 100;
            DateOnly dataAgendamento = new DateOnly(2024, 07, 21);
            var medico = new Medico(nome, crm, valorConsulta);
            var agendamentos =
                new AgendaMedica(dataAgendamento, new List<HorarioDisponivel>
                {
                    new HorarioDisponivel(new TimeOnly(8, 30))
                });

            medico.AdicionarAgendamento(agendamentos);

            // Act & Assert
            Assert.Throws<DomainException>(() => medico.AdicionarAgendamento(agendamentos));
        }

        [Theory]
        [InlineData("00", null)]
        [InlineData("0000/SP", null)]
        public void CriarAgendamentoMedico_Invalido_DeveLancarException(string crm, DateOnly data)
        {
            // Act & Assert
            Assert.Throws<DomainException>(() => new AgendaMedica(data, new List<HorarioDisponivel>()));
        }

        [Fact]
        public void AdicionarHorarios_HorarioJaConfigurado_DeveLancarException()
        {
            // Arrange
            string nome = "Medico Teste";
            string crm = "0000/SP";
            decimal valorConsulta = 100;
            DateOnly dataAgendamento = new DateOnly(2024, 07, 21);
            var medico = new Medico(nome, crm, valorConsulta);
            var horariosDisponiveis = new List<HorarioDisponivel>
            {
                new HorarioDisponivel(new TimeOnly(9, 00)),
                new HorarioDisponivel(new TimeOnly(8, 30)),
            };

            // Arrange, Assert & Act
            Assert.Throws<DomainException>(() => new AgendaMedica(dataAgendamento, horariosDisponiveis));
        }

        [Fact]
        public void AdicionarHorario_HorarioJaConfigurado_DeveLancarException()
        {
            // Arrange
            string nome = "Medico Teste";
            string crm = "0000/SP";
            decimal valorConsulta = 100;
            DateOnly dataAgendamento = new DateOnly(2024, 07, 21);
            var medico = new Medico(nome, crm, valorConsulta);
            var horariosDisponiveis = new List<HorarioDisponivel>
            {
                new HorarioDisponivel(new TimeOnly(9, 30)),
                new HorarioDisponivel(new TimeOnly(8, 30)),
            };

            // Arrange, Assert & Act
            Assert.Throws<DomainException>(() =>
                new AgendaMedica(dataAgendamento, horariosDisponiveis)
                .AdicionarHorario(new HorarioDisponivel(new TimeOnly(10, 20))));
        }
    }
}
