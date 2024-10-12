using GESTOR_DE_ACCIONISTAS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GESTOR_DE_ACCIONISTAS.Service
{
    internal class GrillaService : IComparer<Accionista>
    {
        // Grilla 1 ( Accionistas )
        public void PoblarGrilla1(DataGridView grilla1, List<Accionista> accionistas)
        {
            grilla1.Rows.Clear();

            if (grilla1.Columns.Count == 0)
            {
                grilla1.Columns.Add("Legajo", "Legajo");
                grilla1.Columns.Add("Nombre", "Nombre");
                grilla1.Columns.Add("Apellido", "Apellido");
                grilla1.Columns.Add("Dni", "DNI");
                grilla1.Columns.Add("Clasificacion", "Clasificacion");
            }

            foreach (var accionista in accionistas)
            {
                grilla1.Rows.Add(
                    accionista.Legajo,
                    accionista.Nombre,
                    accionista.Apellido,
                    accionista.Dni,
                    accionista.Clasificacion);
            }
        }

        // Grilla 2 ( Acciones del Accionista seleccionado )
        public void PoblarGrilla2(DataGridView grilla2, List<Accion> acciones)
        {
            grilla2.Rows.Clear();
            if (grilla2.Columns.Count == 0)
            {
                grilla2.Columns.Add("Codigo", "Codigo");
                grilla2.Columns.Add("Empresa", "Empresa");
                grilla2.Columns.Add("Denominacion", "Denominacion");
                grilla2.Columns.Add("Cotizacion Actual", "Cotizacion Actual");
                grilla2.Columns.Add("Cantidad de Acciones", "Cantidad de Acciones");
                grilla2.Columns.Add("Valor Total de la Inversion", "Valor Total de la Inversion");
            }

            foreach (var accion in acciones)
            {
                grilla2.Rows.Add(
                    accion.Codigo,
                    accion.Empresa,
                    accion.Denominacion,
                    accion.Cotizacion_actual,
                    accion.Cantidad_emitida,
                    accion.Cotizacion_actual * accion.Cantidad_emitida);
            }

        }

        // Grilla 3 ( Acciones )
        public void PoblarGrilla3(DataGridView grilla3, List<Accion> acciones)
        {
            grilla3.Rows.Clear();
            if (grilla3.Columns.Count == 0)
            {
                grilla3.Columns.Add("Codigo", "Codigo");
                grilla3.Columns.Add("Empresa", "Empresa");
                grilla3.Columns.Add("Denominacion", "Denominacion");
                grilla3.Columns.Add("Cantidad Emitida", "Cantidad Emitida");
                grilla3.Columns.Add("Cotizacion Actual", "Cotizacion Actual");
            }

            foreach (var accion in acciones)
            {
                grilla3.Rows.Add(
                    accion.Codigo,
                    accion.Empresa,
                    accion.Denominacion,
                    accion.Cantidad_emitida,
                    accion.Cotizacion_actual);
            }
        }

        public void RefrescarTodasLasGrillas(
            DataGridView grillaAccionistas,
            DataGridView grillaAccionAccionista,
            DataGridView grillaAcciones,
            List<Accionista> accionistas,
            List<Accion> acciones,
            Accionista accionista)
        {
            PoblarGrilla1(grillaAccionistas, accionistas);
            PoblarGrilla3(grillaAcciones, acciones);
        }

        public int Compare(Accionista x, Accionista y)
        {
            // Primero se compara por Legajo
            int result = x.Legajo.CompareTo(y.Legajo);

            // Si Legajo es igual, comparar por Nombre
            if (result == 0)
            {
                result = x.Nombre.CompareTo(y.Nombre);
            }

            // Si Nombre es igual, comparar por Apellido
            if (result == 0)
            {
                result = x.Apellido.CompareTo(y.Apellido);
            }

            // Si Apellido es igual, comparar por Dni
            if (result == 0)
            {
                result = x.Dni.CompareTo(y.Dni);
            }

            // Retornar el resultado final
            return result;
        }
    }
}
