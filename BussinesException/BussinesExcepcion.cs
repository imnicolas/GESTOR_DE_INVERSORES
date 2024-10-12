using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GESTOR_DE_ACCIONISTAS.BussinesException
{
    // Implementar excepciones personalizadas
    internal class BussinesExcepcion : Exception
    {
        public BussinesExcepcion(string message) : base(message) { }

        // Primera excepcion personalizada :
        // Todos los datos ingresados por el usuario, no puede ser nulo o vacio .
        // @Datos : 
        // nombre, apellido, dni

        public string datosDeAccionistasIncompletos()
        {
            return "Datos incompletos, por favor complete todos los campos";
        }

        public string datosDeAccionesIncompletos()
        {
            return "Datos incompletos, por favor complete todos los campos";
        }

        // Segunda excepcion personalizada :

        public string clasificacionInvalida()
        {
            return "Clasificación inválida, debe ser 'Comun' o 'Premium'.";
        }
    }
}
