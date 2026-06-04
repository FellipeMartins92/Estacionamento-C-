using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Models
{
    public class EstacionamentoDBContext: DbContext
    {
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<EntradaSaida> EntradaSaida { get; set; }

        public DbSet<Taxa> Taxa { get; set; }

        public EstacionamentoDBContext(DbContextOptions<EstacionamentoDBContext> configuracao)
            : base(configuracao)
        {
            
        }
    }
}
