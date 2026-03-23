using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.DTOs
{
    public class VendaDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataExclusao { get; set; }
        public decimal ValorTotal { get; set; }
        public int QuantidadeBolos { get; set; }
        public string FormaPagamento { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
    }
}
