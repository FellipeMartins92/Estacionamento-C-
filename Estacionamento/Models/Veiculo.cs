using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Estacionamento.Models
{
    [PrimaryKey(nameof(VeiculoPlaca))]
    public class Veiculo
    {
        [Required]
        public String VeiculoPlaca { get; set; } = string.Empty;

        public static bool VeiculoPlacaValida(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return false;

            placa = placa.Trim().ToUpper();

            string placaAntiga = @"^[A-Z]{3}[0-9]{4}$";

            string placaMercosul = @"^[A-Z]{3}[0-9][A-Z][0-9]{2}$";

            return Regex.IsMatch(placa, placaAntiga) || Regex.IsMatch(placa, placaMercosul);
        }
    }    
}
