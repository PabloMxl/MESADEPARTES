using System;
using System.Drawing;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    /// <summary>
    /// Lógica de la ventana principal. La parte visual (controles, posiciones, tamaños)
    /// se edita desde el diseñador gráfico (FormPrincipal.cs [Diseño]).
    /// </summary>
    public partial class FormPrincipal : Form
    {
        private string codigoEnEdicion = null;
        private Button btnEliminar;

        public FormPrincipal()
        {
            InitializeComponent();
            grid.CellDoubleClick += grid_CellDoubleClick;
            CrearBotonEliminar();
            AplicarTemaVisual();
        }

        // Este botón solo aparece cuando hacemos doble clic en un expediente.
        private void CrearBotonEliminar()
        {
            btnEliminar = new Button();
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Text = "Eliminar expediente";
            btnEliminar.Location = new Point(355, 105);
            btnEliminar.Size = new Size(155, 32);
            btnEliminar.BackColor = Color.Firebrick;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Visible = false;
            btnEliminar.Click += btnEliminar_Click;
            gbRegistro.Controls.Add(btnEliminar);
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string codigo = Convert.ToString(grid.Rows[e.RowIndex].Cells["colCodigo"].Value);
            int indice = Repositorio.BuscarPorCodigo(codigo);
            if (indice < 0) return;

            codigoEnEdicion = codigo;
            txtDni.Text = Repositorio.dnis[indice];
            txtNombre.Text = Repositorio.nombres[indice];
            txtAsunto.Text = Repositorio.asuntos[indice];
            cboEstado.SelectedItem = Repositorio.estados[indice];
            btnRegistrar.Text = "Guardar cambios";
            btnLimpiar.Text = "Cancelar edición";
            btnEliminar.Visible = true;
            gbRegistro.Text = "Editar expediente: " + codigo;
            ActualizarTemaEdicion(true);
            lblResumen.Text = "Editando expediente: " + codigo;
            txtDni.Focus();
        }

        private void CancelarEdicion()
        {
            codigoEnEdicion = null;
            btnRegistrar.Text = "Registrar";
            btnLimpiar.Text = "Limpiar";
            btnEliminar.Visible = false;
            gbRegistro.Text = "Registrar expediente";
            ActualizarTemaEdicion(false);
            LimpiarFormulario();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (codigoEnEdicion == null) return;
            int indice = Repositorio.BuscarPorCodigo(codigoEnEdicion);
            if (indice < 0)
            {
                CancelarEdicion();
                MessageBox.Show("El expediente ya no existe.");
                return;
            }

            string codigo = Repositorio.codigos[indice];
            if (MessageBox.Show(
                    "¿Eliminar definitivamente el expediente " + codigo + "?\nEsta acción no se puede deshacer.",
                    "Confirmar eliminación", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            // Guardamos una copia por si hay problemas al escribir el archivo.
            int anterior = Repositorio.cantidad;
            string[] codigos = (string[])Repositorio.codigos.Clone();
            string[] dnis = (string[])Repositorio.dnis.Clone();
            string[] nombres = (string[])Repositorio.nombres.Clone();
            string[] asuntos = (string[])Repositorio.asuntos.Clone();
            string[] fechas = (string[])Repositorio.fechas.Clone();
            string[] estados = (string[])Repositorio.estados.Clone();

            for (int i = indice; i < Repositorio.cantidad - 1; i++)
            {
                Repositorio.codigos[i] = Repositorio.codigos[i + 1];
                Repositorio.dnis[i] = Repositorio.dnis[i + 1];
                Repositorio.nombres[i] = Repositorio.nombres[i + 1];
                Repositorio.asuntos[i] = Repositorio.asuntos[i + 1];
                Repositorio.fechas[i] = Repositorio.fechas[i + 1];
                Repositorio.estados[i] = Repositorio.estados[i + 1];
            }
            int ultimo = --Repositorio.cantidad;
            Repositorio.codigos[ultimo] = null;
            Repositorio.dnis[ultimo] = null;
            Repositorio.nombres[ultimo] = null;
            Repositorio.asuntos[ultimo] = null;
            Repositorio.fechas[ultimo] = null;
            Repositorio.estados[ultimo] = null;

            if (!Persistencia.Guardar())
            {
                Array.Copy(codigos, Repositorio.codigos, codigos.Length);
                Array.Copy(dnis, Repositorio.dnis, dnis.Length);
                Array.Copy(nombres, Repositorio.nombres, nombres.Length);
                Array.Copy(asuntos, Repositorio.asuntos, asuntos.Length);
                Array.Copy(fechas, Repositorio.fechas, fechas.Length);
                Array.Copy(estados, Repositorio.estados, estados.Length);
                Repositorio.cantidad = anterior;
                MessageBox.Show("No se pudo guardar la eliminación.", "Error de archivo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CancelarEdicion();
            MostrarTodos();
            MessageBox.Show("Expediente eliminado: " + codigo, "Eliminación exitosa",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Eventos de los botones

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            // Los tres estados que puede tener un expediente
            cboEstado.Items.AddRange(Repositorio.ESTADOS);
            cboNuevoEstado.Items.AddRange(Repositorio.ESTADOS);
            cboEstado.SelectedIndex = 0;
            cboNuevoEstado.SelectedIndex = 0;

            int cargados = Persistencia.Cargar();
            MostrarTodos();
            lblResumen.Text = "Se cargaron " + cargados + " expediente(s) desde " + Persistencia.RUTA;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string dni = txtDni.Text.Trim();
            string nombre = Validacion.Normalizar(txtNombre.Text).ToUpper();
            string asunto = Validacion.Normalizar(txtAsunto.Text);

            string error = Validacion.ValidarExpediente(dni, nombre, asunto);
            if (error != "")
            {
                MessageBox.Show(error, "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si estamos editando, cambiamos los datos del mismo expediente.
            if (codigoEnEdicion != null)
            {
                int indice = Repositorio.BuscarPorCodigo(codigoEnEdicion);
                if (indice < 0)
                {
                    CancelarEdicion();
                    MessageBox.Show("No se encontró el expediente que estaba editando.");
                    return;
                }

                string codigoEditado = Repositorio.codigos[indice];
                string dniAnterior = Repositorio.dnis[indice];
                string nombreAnterior = Repositorio.nombres[indice];
                string asuntoAnterior = Repositorio.asuntos[indice];
                string estadoAnterior = Repositorio.estados[indice];

                Repositorio.dnis[indice] = dni;
                Repositorio.nombres[indice] = nombre;
                Repositorio.asuntos[indice] = asunto;
                Repositorio.estados[indice] = cboEstado.Text;

                if (!Persistencia.Guardar())
                {
                    Repositorio.dnis[indice] = dniAnterior;
                    Repositorio.nombres[indice] = nombreAnterior;
                    Repositorio.asuntos[indice] = asuntoAnterior;
                    Repositorio.estados[indice] = estadoAnterior;
                    MessageBox.Show("No se pudieron guardar los cambios.", "Error de archivo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CancelarEdicion();
                MostrarTodos();
                MessageBox.Show("Expediente actualizado: " + codigoEditado,
                                "Cambios guardados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string codigo = Repositorio.Registrar(dni, nombre, asunto, cboEstado.Text);
            if (codigo == "")
            {
                MessageBox.Show("Se alcanzó la capacidad máxima de expedientes (" + Repositorio.CAPACIDAD + ").",
                                "Sin espacio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Persistencia.Guardar())
            {
                MessageBox.Show("El expediente se registró, pero no se pudo escribir el archivo.",
                                "Error de archivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MostrarTodos();
            LimpiarFormulario();
            MessageBox.Show("Expediente registrado con el código: " + codigo,
                            "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (codigoEnEdicion != null)
            {
                CancelarEdicion();
                MostrarTodos();
                return;
            }
            LimpiarFormulario();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                MessageBox.Show("Ingrese el código a buscar (ejemplo: EXP-2026-0001).",
                                "Falta el código", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int indice = Repositorio.BuscarPorCodigo(txtBuscar.Text);
            if (indice < 0)
            {
                MessageBox.Show("No se encontró ningún expediente con ese código.",
                                "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            grid.Rows.Clear();
            AgregarFila(indice);
            grid.ClearSelection();
            grid.Rows[0].Selected = true;
            lblResumen.Text = "Expediente encontrado: " + Repositorio.codigos[indice];
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            MostrarTodos();
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            if (Repositorio.cantidad < 2)
            {
                MessageBox.Show("Se necesitan al menos 2 expedientes para ordenar.",
                                "Ordenar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (codigoEnEdicion != null)
                CancelarEdicion();

            Repositorio.OrdenarPorCodigo();
            Persistencia.Guardar();
            MostrarTodos();
            lblResumen.Text = "Expedientes ordenados por código (" + Repositorio.cantidad + ").";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un expediente en la tabla (use Buscar o Mostrar todos).",
                                "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigo = Convert.ToString(grid.CurrentRow.Cells["colCodigo"].Value);
            int indice = Repositorio.BuscarPorCodigo(codigo);

            if (Repositorio.ActualizarEstado(indice, cboNuevoEstado.Text))
            {
                Persistencia.Guardar();
                MostrarTodos();
                lblResumen.Text = "Estado de " + codigo + " actualizado a: " + cboNuevoEstado.Text;
            }
        }

        // Funciones para mostrar y limpiar datos

        private void AgregarFila(int i)
        {
            grid.Rows.Add(Repositorio.codigos[i], Repositorio.dnis[i], Repositorio.nombres[i],
                          Repositorio.asuntos[i], Repositorio.fechas[i], Repositorio.estados[i]);
        }

        private void MostrarTodos()
        {
            grid.Rows.Clear();
            for (int i = 0; i < Repositorio.cantidad; i++)
                AgregarFila(i);
            lblResumen.Text = "Expedientes registrados: " + Repositorio.cantidad;
        }

        private void LimpiarFormulario()
        {
            txtDni.Clear();
            txtNombre.Clear();
            txtAsunto.Clear();
            cboEstado.SelectedIndex = 0;
            txtDni.Focus();
        }
    }
}
