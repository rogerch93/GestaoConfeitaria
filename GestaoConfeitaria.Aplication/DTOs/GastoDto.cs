using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.DTOs
{
    public class GastoDto
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public DateTime DataExclusao { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Usuario { get; set; } = string.Empty;
    }
}
