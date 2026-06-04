using Estacionamento.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Estacionamento.Controllers
{
    public class VeiculoController : Controller
    {
        private readonly EstacionamentoDBContext _context;
        private readonly ILogger<VeiculoController> _logger;

        public VeiculoController(ILogger<VeiculoController> logger, EstacionamentoDBContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Fazer busca no banco e carregar view com o Veículo
        public IActionResult VeiculoInformacoes(string placa)
        {
            var entradasSaidas = _context.EntradaSaida
                .Where(e => e.EntradaSaidaPlaca == placa)
                .OrderByDescending(e => e.EntradaSaidaEntrada)
                .ToList();

            return View(entradasSaidas);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
