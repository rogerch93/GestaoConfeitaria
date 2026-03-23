using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoConfeitaria.Application.Interfaces
{
    public interface IIndicadorService
    {
        Task<string> GerarIndicadoresAsync(DateTime? dataInicio, DateTime? dataFim);
    }
}
