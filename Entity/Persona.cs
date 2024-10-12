using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GESTOR_DE_ACCIONISTAS.Entity
{
    internal abstract class Persona
    {
        private string nombre;

        private string apellido;

        private string dni;

        public Persona(string nombre, string apellido, string dni)
        {
            this.nombre = nombre;
            this.apellido = apellido;
            this.dni = dni;
        }

        public string Nombre
        {
            get => nombre;
            set => nombre = value;
        }

        public string Apellido
        {
            get => apellido;
            set => apellido = value;
        }

        public string Dni
        {
            get => dni;
            set => dni = value;
        }
    }
}
