using System.Data;

namespace calculadora_sqia.Application.Ports.Repositories.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
