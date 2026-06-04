using Estacionamento.Models;
using Estacionamento.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Estacionamento.Controllers
{
    public class TaxaController : Controller
    {

        private readonly EstacionamentoDBContext _context;
        private readonly ILogger<TaxaController> _logger;

        public TaxaController(ILogger<TaxaController> logger, EstacionamentoDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("Taxa")]
        public IActionResult Taxa()
        {
            var viewModel = new TaxaViewModel
            {
                Taxa = new Taxa
                {
                    TaxaVigenciaInicio = DateOnly.FromDateTime(DateTime.Today),
                    TaxaVigenciaFim = DateOnly.FromDateTime(DateTime.Today.AddYears(1))
                },

                Taxas = _context.Taxa.ToList()
            };

            return View(viewModel);
        }

        public IActionResult TaxaCancelarTaxa(int Id)
        {
            
            var taxa = _context.Taxa.FirstOrDefault(x => x.Id == Id);

            if (taxa == null)
            {
                return BadRequest("Taxa inválida.");
            }

            taxa.TaxaAtiva = false;

            _context.SaveChanges();

            return RedirectToAction("Taxa");
        }

        public IActionResult SalvarTaxa(Taxa taxa)
        {
            if (!Models.Taxa.IsTaxaValida(taxa)){
                return BadRequest("Taxa inválida.");
            }

            bool conflito = _context.Taxa.Any(t =>
                t.Id != taxa.Id &&
                t.TaxaAtiva == true &&
                taxa.TaxaVigenciaInicio <= t.TaxaVigenciaFim &&
                taxa.TaxaVigenciaFim >= t.TaxaVigenciaInicio
             );

            if (conflito)
                return BadRequest("Taxa inválida.");

            taxa.TaxaAtiva = true;
            _context.Taxa.Add(taxa);
            _context.SaveChanges();

            return RedirectToAction(nameof(Taxa));
        }
    }
}
