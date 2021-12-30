using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireSerilogEx.Hangfire.Jobs
{
    public class BuscarHoraAtual
    {
        public object Run()
        {
            return "Horario de execução: " + DateTime.Now.ToString();
        }
    }
}
