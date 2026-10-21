using System.Drawing;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    // Aquí puse los colores para no llenar el formulario principal de estilos.
    public partial class FormPrincipal
    {
        private readonly Color azulMarino = Color.FromArgb(22, 40, 70);
        private readonly Color azul = Color.FromArgb(29, 92, 170);
        private readonly Color verde = Color.FromArgb(16, 125, 86);
        private readonly Color rojo = Color.FromArgb(179, 42, 54);
        private readonly Color violeta = Color.FromArgb(99, 69, 155);
        private readonly Color fondo = Color.FromArgb(241, 246, 252);
        private readonly Color texto = Color.FromArgb(32, 48, 70);

        // Colores de botones, campos y tabla
        private void AplicarTemaVisual()
        {
            BackColor = fondo;
            ForeColor = texto;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            Text = "Mesa de Partes Digital | Gestión de expedientes";

            EstilizarGrupo(gbRegistro);
            EstilizarGrupo(gbConsulta);

            EstilizarCampo(txtDni);
            EstilizarCampo(txtNombre);
            EstilizarCampo(txtAsunto);
            EstilizarCampo(txtBuscar);
            EstilizarCombo(cboEstado);
            EstilizarCombo(cboNuevoEstado);

            EstilizarBoton(btnRegistrar, verde);
            EstilizarBoton(btnLimpiar, Color.FromArgb(95, 108, 125));
            EstilizarBoton(btnBuscar, azul);
            EstilizarBoton(btnMostrar, azulMarino);
            EstilizarBoton(btnOrdenar, violeta);
            EstilizarBoton(btnActualizar, verde);
            EstilizarBoton(btnEliminar, rojo);

            lblResumen.ForeColor = azulMarino;
            lblResumen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.GridColor = Color.FromArgb(220, 230, 240);
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = azulMarino;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = azulMarino;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grid.ColumnHeadersHeight = 36;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = texto;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 100, 181);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 243, 252);
            grid.AlternatingRowsDefaultCellStyle.ForeColor = texto;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 100, 181);
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
            grid.RowTemplate.Height = 29;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;

            ActualizarTemaEdicion(false);
        }

        private void EstilizarGrupo(GroupBox grupo)
        {
            grupo.BackColor = Color.White;
            grupo.ForeColor = azulMarino;
            grupo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            foreach (Control control in grupo.Controls)
            {
                if (control is Label)
                {
                    control.ForeColor = texto;
                    control.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                }
            }
        }

        private void EstilizarCampo(TextBox campo)
        {
            campo.BackColor = Color.FromArgb(250, 252, 255);
            campo.ForeColor = texto;
            campo.BorderStyle = BorderStyle.FixedSingle;
            campo.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        private void EstilizarCombo(ComboBox combo)
        {
            combo.BackColor = Color.White;
            combo.ForeColor = texto;
            combo.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
        }

        private void EstilizarBoton(Button boton, Color color)
        {
            boton.BackColor = color;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.FlatAppearance.MouseOverBackColor = ControlPaint.Light(color, 0.18F);
            boton.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(color, 0.12F);
            boton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;
            boton.UseVisualStyleBackColor = false;
        }

        private void ActualizarTemaEdicion(bool editando)
        {
            gbRegistro.ForeColor = editando ? violeta : azulMarino;
            btnRegistrar.BackColor = editando ? azul : verde;
            btnEliminar.Visible = editando;
        }
    }
}
