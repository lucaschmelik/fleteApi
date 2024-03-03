using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fleteApi.Models
{
    public class DiaCalendario
    {
        public bool Status { get; set; }
        public int NumeroDia { get; set; }
        public int NumeroMes { get; set; }
        public List<DiaCalendarioItem> Items { get; set; }
    }
}
