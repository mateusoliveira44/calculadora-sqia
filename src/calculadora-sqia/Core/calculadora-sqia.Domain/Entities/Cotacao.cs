using calculadora_sqia.Core;

namespace calculadora_sqia.Domain.Entities
{
    public class Cotacao : Entity
    {
        public DateTime Data { get; private set; }
        public string Indexador { get; private set; }
        public decimal Valor { get; private set; }

        public Cotacao(int id, DateTime data, string indexador, decimal valor) : base(id)
        {
            Data = data;
            Indexador = indexador;
            Valor = valor;
        }
    }
}
