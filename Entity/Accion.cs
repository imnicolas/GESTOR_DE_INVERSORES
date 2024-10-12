using GESTOR_DE_ACCIONISTAS.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GESTOR_DE_ACCIONISTAS.Entity
{

    internal class Accion : IEnumerable<string>
    {
        public event EventHandler<CotizacionChangedEventArgs> CotizacionCambiada;
        private string codigo;
        private string empresa;
        private string denominacion;
        private int cantidad_emitida;
        private decimal cotizacion_actual;

        public Accion(string empresa, string denominacion, int cantidad_emitida, decimal cotizacion_actual)
        {
            this.codigo = generarCodigo();
            this.empresa = empresa;
            this.denominacion = denominacion;
            this.cantidad_emitida = cantidad_emitida;
            this.cotizacion_actual = cotizacion_actual;
        }

        public string Codigo
        {
            get => codigo;
            set => codigo = value;
        }

        public string Empresa
        {
            get => empresa;
            set => empresa = value;
        }

        public string Denominacion
        {
            get => denominacion;
            set => denominacion = value;
        }

        public int Cantidad_emitida
        {
            get => cantidad_emitida;
            set => cantidad_emitida = value;
        }

        public decimal Cotizacion_actual
        {
            get => cotizacion_actual;
            set
            {
                if (cotizacion_actual != value)
                {
                    cotizacion_actual = value;
                    MessageBox.Show(
                        $"Cotización cambiada a: {cotizacion_actual}\n" +
                        $"Código de la acción: {codigo}");
                    OnCotizacionCambiada(new CotizacionChangedEventArgs(cotizacion_actual));
                }
            }
        }

        protected virtual void OnCotizacionCambiada(CotizacionChangedEventArgs e)
        {
            CotizacionCambiada?.Invoke(this, e);
        }

        public string generarCodigo()
        {
            Random random = new Random();
            string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numeros = "0123456789";
            string codigo = "";
            for (int i = 0; i < 4; i++)
            {
                codigo += letras[random.Next(0, letras.Length)];
            }
            codigo += "-";
            for (int i = 0; i < 4; i++)
            {
                codigo += numeros[random.Next(0, numeros.Length)];
            }
            codigo += "-";
            for (int i = 0; i < 4; i++)
            {
                if (i % 2 == 0)
                {
                    codigo += letras[random.Next(0, letras.Length)];
                }
                else
                {
                    codigo += numeros[random.Next(0, numeros.Length)];
                }
            }
            return codigo;
        }

        public IEnumerator<string> GetEnumerator()
        {
            string[] partes = Codigo.Split('-'); // Separa por guiones
            foreach (var parte in partes)
            {
                yield return parte; // Devuelve cada parte sin los guiones
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
