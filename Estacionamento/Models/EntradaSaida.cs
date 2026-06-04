using Microsoft.EntityFrameworkCore;

namespace Estacionamento.Models
{
    [PrimaryKey(nameof(Id))]
    public class EntradaSaida
    {
        public int Id { get; set; }

        public DateTime EntradaSaidaEntrada { get; set; }
        public String EntradaSaidaPlaca{ get; set; }

        public DateTime? EntradaSaidaSaida { get; set; }

        public Double? EntradaSaidaValor { get; set; }

        public EntradaSaida(string EntradaSaidaPlaca)
        {
            this.EntradaSaidaPlaca = EntradaSaidaPlaca;
        }     

        public static Double CalculaTaxa(DateTime DataEntrada, DateTime DataSaida, Taxa taxa)
        {
            var ValorFinal = 0.0;
            const double tolerancia = 10.0 / 60.0;

            TimeSpan diferenca = DataSaida - DataEntrada;
            double horas = diferenca.TotalHours;

            //Meia Hora
            if (horas <= 0.5)
            {
                //Garante que não gere valores com 3 decimais
                return Math.Round(taxa.TaxaValor / 2, 2);
            }

            //Garante que a "primeira hora" já foi debitada
            //Prevê casos onde o carro ficou por mais de 30 minutos e menos de uma hora
            ValorFinal = 1 * taxa.TaxaValor;

            while (horas > 1)
            {
                //Remove uma hora
                horas -= 1;

                //Considera os 10 minutos de tolerância
                if (horas > tolerancia)
                {
                    ValorFinal += taxa.TaxaValorAdicional;
                }
            }

            return ValorFinal;
        }
    }
}
