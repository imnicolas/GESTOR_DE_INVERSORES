using GESTOR_DE_ACCIONISTAS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTOR_DE_ACCIONISTAS.Entity
{
    internal class Comun : Accionista, CalcularComision
    {
        private decimal tasa_comision = 1;
        private decimal comision_total;

        public Comun(string nombre, string apellido, string dni) : base(nombre, apellido, dni) { }

        public decimal Tasa_comision
        {
            get => tasa_comision;
            set => tasa_comision = value;
        }

        public decimal Comision_total
        {
            get => comision_total;
            set => comision_total = value;
        }

        /// <summary>
        /// Cada vez que realizan una operación de compra o venta se les cobra una comisión del 1% sobre el total.
        /// </summary>
        public decimal CalcularComision()
        {
            decimal comision = 0;
            foreach (Accion accion in Acciones)
            {
                comision += accion.Cotizacion_actual * accion.Cantidad_emitida * 0.01m;
            }
            return comision;
        }
    }
}
