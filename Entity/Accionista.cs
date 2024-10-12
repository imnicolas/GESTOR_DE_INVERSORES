using GESTOR_DE_ACCIONISTAS.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTOR_DE_ACCIONISTAS.Entity
{
    /// <summary>
    /// Un accionista puede ser de 2 tipos, Comun o Premium
    /// </summary>
    
    /// <summary>
    /// Los inversores invierten en acciones de empresas. Cada inversor puede tener
    /// inversiones en distintas acciones y de cada una de ellas poseer distintas cantidades
    /// </summary>
    internal class Accionista : Persona
    {

        private string legajo;
        private List<Accion> acciones;
        private int cantidad_acciones;
        public string clasificacion;

        public Accionista(string nombre, string apellido, string dni, string clasificacion) : base(nombre, apellido, dni)
        {
            this.legajo = generarLegajo();
            this.clasificacion = clasificacion;
            this.acciones = new List<Accion>();
        }

        public Accionista(string nombre, string apellido, string dni) : base(nombre, apellido, dni)
        {
            this.legajo = generarLegajo();
            this.acciones = new List<Accion>();
        }

        public string Legajo
        {
            get => legajo;
            set => legajo = value;
        }

        public List<Accion> Acciones
        {
            get => acciones;
            set => acciones = value;
        }

        public string Clasificacion
        {
            get => clasificacion;
            set => clasificacion = value;
        }
        
        // Agregar o actualizar una acción
        public void agregarAccion(Accion accion)
        {
            // Verificar si el accionista ya tiene esa acción
            var accionExistente = acciones.FirstOrDefault(a => a.Codigo == accion.Codigo);

            if (accionExistente != null)
            {
                // Actualizar la cantidad si ya existe
                accionExistente.Cantidad_emitida += accion.Cantidad_emitida;
            }
            else
            {
                // Agregar la acción si no existe
                acciones.Add(accion);
            }
        }

        // Remover una acción o reducir la cantidad
        public void eliminarAccion(Accion accion, int cantidad)
        {
            var accionExistente = acciones.FirstOrDefault(a => a.Codigo == accion.Codigo);

            if (accionExistente != null)
            {
                if (cantidad >= accionExistente.Cantidad_emitida)
                {
                    // Si la cantidad  >= a la cantidad emitida, eliminar la acción
                    acciones.Remove(accionExistente);
                }
                else
                {
                    accionExistente.Cantidad_emitida -= cantidad;
                }
            }
        }

        // Actualizar una acción en la lista
        public void actualizarAccion(Accion accion)
        {
            int index = acciones.FindIndex(a => a.Codigo == accion.Codigo);
            if (index >= 0)
            {
                acciones[index] = accion;
            }
        }

        public string generarLegajo()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
