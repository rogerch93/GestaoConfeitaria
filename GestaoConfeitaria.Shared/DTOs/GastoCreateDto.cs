using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Shared.DTOs
{
    public class GastoCreateDto
    {
        public DateTime? Data { get; set; }
        public DateTime? DataExclusao { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Usuario { get; set; } = "root";
    }
}
