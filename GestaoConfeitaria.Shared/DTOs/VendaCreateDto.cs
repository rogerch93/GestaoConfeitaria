using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Shared.DTOs
{
    public class VendaCreateDto
    {
        public decimal ValorTotal { get; set; }
        public int QuantidadeBolos { get; set; }
        public string FormaPagamento { get; set; } = string.Empty;
        public string Usuario { get; set; } = "root";
    }
}
