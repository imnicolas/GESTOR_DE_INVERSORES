using GESTOR_DE_ACCIONISTAS.Entity;
using System;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Collections.Generic;
using GESTOR_DE_ACCIONISTAS.Service;
using GESTOR_DE_ACCIONISTAS.Event;
using System.Linq;
using GESTOR_DE_ACCIONISTAS.BussinesException;

namespace GESTOR_DE_ACCIONISTAS
{
    // TO DO Refactorizar. Llevar esta lógica de modificar datos de Accionista a un método en Accionista.

    // TO DO Refactorizar. Llevar esta lógica de modificar datos de Accion a un método en Accion.

    // TO DO EN RefreshViews() para refrescar correctamente con lo que hay .
    // Actualmente al borrar una acción de la grilla 3 no se actualiza la grilla 2 (acciones del accionistas deberian borrarse ?)
    public partial class Form1 : Form
    {
        List<Accionista> accionistas = new List<Accionista>();
        List<Accion> acciones = new List<Accion>();
        GrillaService grillaService = new GrillaService();

        private bool isAscendingOrder = true;

        public Form1()
        {
            InitializeComponent();

            // Suscribir al Evento en Grilla 1 (Sort)
            dataGridView1.ColumnHeaderMouseClick += OrdenarGrilla1Event;

            // Eventos
            // Mostrar información en Grilla 2
            dataGridView1.SelectionChanged += mostrarInfoGrilla2Event;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Crear Accionista

            try
            {
                string name = Interaction.InputBox("Nombre (: string)");
                string apellido = Interaction.InputBox("Apellido (: string)");
                string dni = Interaction.InputBox("Dni (: string)");

                // Seleccionar Clasificación
                string[] opcionesClasificacion = { "Comun", "Premium" };
                string clasificacion = Interaction.InputBox("Seleccione Clasificación (Comun/Premium):", "Clasificación", "Comun");

                if (!validadorDeEntradasAccionistas(name, apellido, dni, clasificacion))
                {
                    throw new BussinesExcepcion(new BussinesExcepcion("").datosDeAccionistasIncompletos());
                }

                Accionista accionista;
                Comun comun;
                Premium premium;

                if (clasificacion == "Comun")
                {
                    comun = new Comun(name, apellido, dni);
                    comun.Clasificacion = clasificacion;
                    accionista = comun;
                }
                else
                {
                    premium = new Premium(name, apellido, dni);
                    premium.Clasificacion = clasificacion;
                    accionista = premium;
                }

                // Agregar el accionista a la lista
                accionistas.Add(accionista);
                
                grillaService.PoblarGrilla1(dataGridView1, accionistas);

            }
            catch (BussinesExcepcion ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        public Boolean validadorDeEntradasAccionistas(string inp1, string inp2, string inp3, string inp4)
        {
            if (string.IsNullOrEmpty(inp1) || string.IsNullOrEmpty(inp2) || string.IsNullOrEmpty(inp3))
            {
                return false;
            }
            else if(inp4 != "Comun" && inp4 != "Premium")
            {
                return false;
            }

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Crear Accion
            try
            {
                string empresa = Interaction.InputBox("Empresa (: string)");
                string denominacion = Interaction.InputBox("Denominacion (: string)");
                int cantidad_emitida = Convert.ToInt32(Interaction.InputBox("Cantidad Emitida (: int)"));
                decimal cotizacion_actual = Convert.ToDecimal(Interaction.InputBox("Cotizacion Actual (: decimal)"));
                Accion accion = new Accion(empresa, denominacion, cantidad_emitida, cotizacion_actual);

                if (!validadorDeEntradasDeAccion(empresa, denominacion, cantidad_emitida, cotizacion_actual))
                {
                    throw new BussinesExcepcion(new BussinesExcepcion("").datosDeAccionesIncompletos());
                }

                acciones.Add(accion);

                grillaService.PoblarGrilla3(dataGridView3, acciones);
            }
            catch (BussinesExcepcion ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public Boolean validadorDeEntradasDeAccion(string inp1, string inp2, int inp3, decimal inp4)
        {
            if (string.IsNullOrEmpty(inp1) || string.IsNullOrEmpty(inp2))
            {
                return false;
            }
            else if (inp3 <= 0 || inp4 <= 0)
            {
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Borrar Accionista

            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("No hay ningún Accionista seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int index = dataGridView1.CurrentCell.RowIndex;

            // Remover
            Accionista accionistaFinded = accionistas[index];
            accionistas.RemoveAt(index);

            RefreshViews(accionistaFinded);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Borrar Accion

            if (dataGridView3.CurrentRow == null)
            {
                MessageBox.Show("No hay ninguna accion seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int index = dataGridView3.CurrentCell.RowIndex;
            Accion accionFinded = acciones[index];

            // Eliminar la accion de cada accionista que la posee
            foreach (var accionista in accionistas)
            {
                accionista.eliminarAccion(accionFinded, accionFinded.Cantidad_emitida);
            }

            // Remover
            acciones.RemoveAt(index);

            RefreshViews(accionista: null);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Modificar Accionista seleccionado

            int index = dataGridView1.CurrentCell.RowIndex;
            Accionista accionistaFinded = accionistas[index];

            string nuevoNombre = Interaction.InputBox("Modificar Nombre:", accionistaFinded.Nombre);
            string nuevoApellido = Interaction.InputBox("Modificar Apellido:", accionistaFinded.Apellido);
            string nuevoDni = Interaction.InputBox("Modificar DNI:", accionistaFinded.Dni);

            // Actualizar los datos del Accionista
            accionistaFinded.Nombre = nuevoNombre;
            accionistaFinded.Apellido = nuevoApellido;
            accionistaFinded.Dni = nuevoDni;

            grillaService.PoblarGrilla1(dataGridView1, accionistas);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Modificar Accion seleccionada

            int index = dataGridView3.CurrentCell.RowIndex;
            Accion accionFinded = acciones[index];

            string nuevaEmpresa = Interaction.InputBox("Modificar Empresa:", accionFinded.Empresa);
            string nuevaDenominacion = Interaction.InputBox("Modificar Denominacion:", accionFinded.Denominacion);
            int nuevaCantidadEmitida = Convert.ToInt32(Interaction.InputBox("Modificar Cantidad Emitida:", accionFinded.Cantidad_emitida.ToString()));
            decimal nuevaCotizacionActual = Convert.ToDecimal(Interaction.InputBox("Modificar Cotización:", accionFinded.Cotizacion_actual.ToString()));

            // Actualizar los datos de la accion
            accionFinded.Empresa = nuevaEmpresa;
            accionFinded.Denominacion = nuevaDenominacion;
            accionFinded.Cantidad_emitida = nuevaCantidadEmitida;
            accionFinded.Cotizacion_actual = nuevaCotizacionActual; // Dispara el evento

            // Accionista seleccionado
            Accionista accionistaSeleccionado = null;
            if (dataGridView1.CurrentRow != null)
            {
                int indexAccionista = dataGridView1.CurrentCell.RowIndex;
                accionistaSeleccionado = accionistas[indexAccionista];
            }

            // Refrescar las grillas, pasando el accionista solo SI esta seleccionado
            RefreshViews(accionistaSeleccionado);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Comprar Acciones

            int indexAccionista = dataGridView1.CurrentCell.RowIndex;
            int indexAccion = dataGridView3.CurrentCell.RowIndex;

            Accionista accionista = accionistas[indexAccionista];
            Accion accionGlobal = acciones[indexAccion];

            // Pedir la cantidad a comprar
            int cantidadCompra = Convert.ToInt32(Interaction.InputBox("Cantidad de acciones a comprar:", "Compra Acciones"));

            // Validar que no se superen las acciones emitidas
            if (cantidadCompra <= accionGlobal.Cantidad_emitida)
            {
                // Buscar si el accionista ya tiene esa accion
                var accionExistente = accionista.Acciones.FirstOrDefault(a => a.Codigo == accionGlobal.Codigo);

                if (accionExistente != null)
                {
                    // Actualizar la cantidad si ya tiene la accion
                    accionExistente.Cantidad_emitida += cantidadCompra;
                }
                else
                {
                    // Crear una nueva copia de la accion y agregarla al accionista
                    Accion nuevaAccion = new Accion(accionGlobal.Empresa, accionGlobal.Denominacion, cantidadCompra, accionGlobal.Cotizacion_actual)
                    {
                        Codigo = accionGlobal.Codigo // Copiamos el código original de la accion
                    };

                    accionista.agregarAccion(nuevaAccion);
                }

                // Restar la cantidad comprada del total emitido en la lista global de acciones
                accionGlobal.Cantidad_emitida -= cantidadCompra;

                RefreshViews(accionista);

                ActualizarComisiones();
            }
            else
            {
                MessageBox.Show("La cantidad supera las acciones emitidas", "Error de Compra", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // Vender Acciones

            int indexAccionista = dataGridView1.CurrentCell.RowIndex;
            int indexAccion = dataGridView2.CurrentCell.RowIndex;

            Accionista accionista = accionistas[indexAccionista];
            Accion accionAccionista = accionista.Acciones[indexAccion];
            Accion accionGlobal = acciones.FirstOrDefault(a => a.Codigo == accionAccionista.Codigo);

            // Pedir la cantidad a vender
            int cantidadVenta = Convert.ToInt32(Interaction.InputBox("Cantidad de acciones a vender:", "Venta Acciones"));

            // Validar que el accionista cuenta con acciones para la venta
            if (cantidadVenta <= accionAccionista.Cantidad_emitida)
            {
                // Si esta vendiendo el total de acciones, elimino la accion para el Accionista
                if (cantidadVenta == accionAccionista.Cantidad_emitida)
                {
                    accionista.eliminarAccion(accionAccionista, cantidadVenta);
                }
                else
                {
                    // Reducir la cantidad de acciones que posee el accionista
                    accionAccionista.Cantidad_emitida -= cantidadVenta;
                }

                // Sumar la cantidad vendida al total emitido en la lista global de acciones
                accionGlobal.Cantidad_emitida += cantidadVenta;

                RefreshViews(accionista);

                ActualizarComisiones();
            }
            else
            {
                MessageBox.Show("El inversor no posee suficientes acciones", "Error de Venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshViews(Accionista accionista)
        {
            // Refrescar la grilla 1 y la grilla 3 siempre
            grillaService.RefrescarTodasLasGrillas(dataGridView1, dataGridView2, dataGridView3, accionistas, acciones, accionista);

            // Si el accionista es null, no refrescar la grilla 2
            if (accionista != null)
            {
                grillaService.PoblarGrilla2(dataGridView2, accionista.Acciones);
            }

            // Refrescar siempre la grilla 3 con las acciones globales
            grillaService.PoblarGrilla3(dataGridView3, acciones);
        }

        private void ActualizarComisiones()
        {
            decimal comisionComunTotal = 0;
            decimal comisionPremium20kTotal = 0;
            decimal comisionPremiumSuperadoTotal = 0;

            foreach (var accionista in accionistas)
            {
                if (accionista is Comun)
                {
                    comisionComunTotal += ((Comun)accionista).CalcularComision();
                }
                else if (accionista is Premium)
                {
                    Premium premium = (Premium)accionista;
                    foreach (var accion in premium.Acciones)
                    {
                        decimal totalInversion = accion.Cotizacion_actual * accion.Cantidad_emitida;
                        if (totalInversion <= 20000)
                        {
                            comisionPremium20kTotal += totalInversion * 0.01m;
                        }
                        else
                        {
                            comisionPremium20kTotal += 20000 * 0.01m;
                            comisionPremiumSuperadoTotal += (totalInversion - 20000) * 0.005m;
                        }
                    }
                }
            }

            // Actualiza los labels
            ActualizarComisionesUI(comisionComunTotal, comisionPremium20kTotal, comisionPremiumSuperadoTotal);
        }

        private void ActualizarComisionesUI(decimal comisionComun, decimal comisionPremium20k, decimal comisionPremiumSuperado)
        {
            label5.Text = $"Total Comision Comun : ${comisionComun}";
            label6.Text = $"Total Comision Premium <= $20.000 : {comisionPremium20k}";
            label7.Text = $"Total Comision Premium > $20.000 : {comisionPremiumSuperado}";
            label8.Text = $"Total General Comisiones : ${comisionComun + comisionPremium20k + comisionPremiumSuperado}";
        }

        // EVENTOS
        private void OrdenarGrilla1Event(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Obtener el nombre de la columna clickeada
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            // Determinar el criterio de ordenamiento en base a la columna
            Func<Accionista, object> orderCriteria;

            switch (columnName)
            {
                case "Legajo":
                    orderCriteria = a => a.Legajo;
                    break;
                case "Nombre":
                    orderCriteria = a => a.Nombre;
                    break;
                case "Apellido":
                    orderCriteria = a => a.Apellido;
                    break;
                case "Dni":
                    orderCriteria = a => a.Dni;
                    break;
                default:
                    return; // No se ordena si la columna no es relevante
            }

            // Ordenar la lista de accionistas
            if (isAscendingOrder)
            {
                accionistas = accionistas.OrderBy(orderCriteria).ToList();
            }
            else
            {
                accionistas = accionistas.OrderByDescending(orderCriteria).ToList();
            }

            // Alternar el orden para la próxima vez que se haga clic en la columna
            isAscendingOrder = !isAscendingOrder;

            // Refrescar la grilla con la lista ordenada
            grillaService.PoblarGrilla1(dataGridView1, accionistas);
        }

        private void mostrarInfoGrilla2Event(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Recuperar el legajo del accionista seleccionado
                string legajo = dataGridView1.SelectedRows[0].Cells["Legajo"].Value.ToString();

                // Encontrar al accionista
                Accionista accionistaSeleccionado = accionistas.Find(a => a.Legajo == legajo);

                if (accionistaSeleccionado != null)
                {
                    // Poblar la grilla 2 con las acciones del accionista seleccionado
                    grillaService.PoblarGrilla2(dataGridView2, accionistaSeleccionado.Acciones);
                }
            }
        }

        // Nombre de la Empresa || Sistema.
        private void label1_Click(object sender, EventArgs e){}

        // Total recaudado por operaciones de los clientes comunes.
        private void label5_Click(object sender, EventArgs e){}

        // Total recaudado en concepto de comisiones por operaciones de los clientes premium <= $20.000.
        private void label6_Click(object sender, EventArgs e){}

        // Total recaudado en concepto de comisiones por las operaciones de los clientes premium > $20.000.
        private void label7_Click(object sender, EventArgs e){}

        // Total general percibido en concepto de comisiones.
        private void label8_Click(object sender, EventArgs e){}

        /// <summary>
        /// (Grilla 1)
        /// Muestra la lista de todos los inversores (todos sus datos).
        /// </summ  ary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e){}

        /// <summary>
        /// (Grilla 2)
        /// Las acciones que posea el inversor selecciondo en la Grilla 1 
        /// (todos los datos de las accion + cuantas acciones posee el inversor + el valor total de la
        /// inversión[total acciones * cotización]).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e){}

        /// <summary>
        /// (Grilla 3)
        /// Todas las acciones (todos sus datos) en la que el inversor puede invertir.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView3.CurrentRow == null) return;

            // Obtener el índice de la acción seleccionada en la grilla 3
            int indexAccion = dataGridView3.CurrentCell.RowIndex;

            // Obtener la acción seleccionada de la lista de acciones
            Accion accionSeleccionada = acciones[indexAccion];

            // Recorrer las partes del código de la acción sin los guiones
            string codigoSinGuiones = string.Join("", accionSeleccionada);

            // Mostrar el código sin guiones (ejemplo en MessageBox, o puedes asignarlo a un Label)
            codigo_accion.Text = $"Codigo de la Accion seleccionada : {codigoSinGuiones}";
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e){}

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e){}

        private void codigo_accion_Click(object sender, EventArgs e){}
    }
}
