using Estacionamento.Models;

namespace Estacionamento.ViewModel
{
    public class TaxaViewModel
    {
        public Taxa Taxa { get; set; } = new();
        public List<Taxa> Taxas { get; set; } = new();
    }
}
