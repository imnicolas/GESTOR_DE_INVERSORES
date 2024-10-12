using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTOR_DE_ACCIONISTAS.Event
{
    internal class CotizacionChangedEventArgs
    {
        public decimal Cotizacion_actual { get; }

        public CotizacionChangedEventArgs(decimal cotizacion_actual)
        {
            this.Cotizacion_actual = cotizacion_actual;
        }
    }
}
