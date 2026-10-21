using System;
using System.Windows.Forms;

namespace MesaPartesDigital
{
    /// <summary>Punto de entrada de la aplicación.</summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormPrincipal());
        }
    }
}
