using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTipoCambio.Model
{
    public class OperacionTC
    {
        public int id { get; set; }
        public string monedaOrigen { get; set; }
        public string monedaDestino { get; set; }
        public decimal monto { get; set; }
        public decimal montoTc { get; set; }
        public string accion { get; set; }
        public decimal tipoCambio { get; set; }
        public DateTime fechaOperacion { get; set; }
    }
}
