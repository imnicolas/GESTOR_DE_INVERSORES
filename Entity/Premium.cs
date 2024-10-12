using GESTOR_DE_ACCIONISTAS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GESTOR_DE_ACCIONISTAS.Entity
{
    internal class Premium : Accionista, CalcularComision
    {
        private decimal tasa_comision = 1;
        private decimal comision_total;

        public Premium(string nombre, string apellido, string dni) : base(nombre, apellido, dni) { }

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

        /// Calcular la comision de acuerdo a las condiciones del Premium el 1% por compras y ventas hasta 20.000 pesos
        /// Superada esa base sobre el resto abonan la mitad del porcentaje consignado anteriormente
        public decimal CalcularComision()
        {
            decimal comision = 0;
            decimal base_comision = 20000;
            foreach (Accion accion in Acciones)
            {
                decimal total = accion.Cotizacion_actual * accion.Cantidad_emitida;
                if (total <= base_comision)
                {
                    comision += total * 0.01m;
                }
                else
                {
                    comision += base_comision * 0.01m + (total - base_comision) * 0.005m;
                }
            }
            return comision;
        }

        public void comprarAccion(Accion accion)
        {
            Acciones.Add(accion);
        }

        public void venderAccion(Accion accion)
        {
            Acciones.Remove(accion);
        }

    }
}
