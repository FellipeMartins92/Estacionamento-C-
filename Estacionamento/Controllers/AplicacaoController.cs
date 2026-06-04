using Estacionamento.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Estacionamento.Controllers
{
    public class AplicacaoController : Controller
    {

        private readonly EstacionamentoDBContext _context;
        private readonly ILogger<AplicacaoController> _logger;

        public AplicacaoController(ILogger<AplicacaoController> logger, EstacionamentoDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Aplicacao()
        {
            var lista = _context.EntradaSaida
                .OrderByDescending(x => x.EntradaSaidaEntrada)
                .ToList();

            return View(lista);
        }
        public IActionResult AplicacaoRegistrarEntrada(string placa)
        {            

            if (!Veiculo.VeiculoPlacaValida(placa)){
                return BadRequest("Placa de veículo inválida.");
            }

            placa = placa.ToUpper();

            var entradaAberta = _context.EntradaSaida
                .FirstOrDefault(e =>
                    e.EntradaSaidaPlaca == placa &&
                    e.EntradaSaidaSaida == null);

            if (entradaAberta != null)
            {
                return BadRequest("Veículo já está no estacionamento.");
            }

            var veiculo = _context.Veiculos.Find(placa);

            if (veiculo == null)
            {
                veiculo = new Veiculo();
                veiculo.VeiculoPlaca = placa;
                

                _context.Veiculos.Add(veiculo);
                _context.SaveChanges();
            }

            var entradaSaida = new EntradaSaida(placa)
            {
                EntradaSaidaEntrada = DateTime.Now
            };

            _context.EntradaSaida.Add(entradaSaida);
            _context.SaveChanges();

            return RedirectToAction("Aplicacao");
        }

        public IActionResult AplicacaoRegistrarSaida(int id)
        {
            
            var entradaSaida = _context.EntradaSaida.Find(id);            

            if (entradaSaida == null)
            {
                return NotFound();
            }

            if (entradaSaida.EntradaSaidaSaida != null)
            {
                return BadRequest("Saída já registrada.");
            }

            entradaSaida.EntradaSaidaSaida = DateTime.Now;

            var taxa = _context.Taxa
                .Where(t => t.TaxaAtiva == true)
                .OrderBy(t => Math.Abs(
                    (t.TaxaVigenciaFim.ToDateTime(TimeOnly.MinValue) -
                        entradaSaida.EntradaSaidaEntrada).TotalDays))
                .FirstOrDefault();

            if (taxa == null)
            {
                taxa = new Taxa
                {
                    TaxaValor = 5,
                    TaxaValorAdicional = 6
                };
            }

            entradaSaida.EntradaSaidaValor =
                Models.EntradaSaida.CalculaTaxa(
                    entradaSaida.EntradaSaidaEntrada,
                    entradaSaida.EntradaSaidaSaida.Value,
                    taxa);

            _context.SaveChanges();

            return RedirectToAction("Aplicacao");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
