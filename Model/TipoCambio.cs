using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTipoCambio.Model
{
    public class TipoCambio
    {
        public int id { get; set; }
        public string monedaOrigen { get; set; }
        public string monedaDestino { get; set; }
        public decimal tcCompra { get; set; }
        public decimal tcVenta { get; set; }
    }
}
