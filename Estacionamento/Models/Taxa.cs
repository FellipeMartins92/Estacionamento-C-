using System.Data.SqlTypes;

namespace Estacionamento.Models
{
    public class Taxa
    {
        public int Id { get; set; } 

        public Double TaxaValor { get; set; }
        public Double TaxaValorAdicional { get; set; }

        public DateOnly TaxaVigenciaInicio { get; set; }

        public DateOnly TaxaVigenciaFim { get; set; }

        public Boolean TaxaAtiva { get; set; }

        public static bool IsTaxaValida(Taxa taxa)
        {
            if (taxa.TaxaVigenciaFim <= taxa.TaxaVigenciaInicio)
            {
                return false;
            }

            if (taxa.TaxaVigenciaInicio < DateOnly.FromDateTime(DateTime.Today))
            {
                return false;
            }

            if (taxa.TaxaValor <= 0)
            {
                return false;
            }

            if (taxa.TaxaValorAdicional <= 0)
            {
                return false;
            }

            return true;
        }
        
    }
}
