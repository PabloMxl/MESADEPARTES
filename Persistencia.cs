using System;
using System.IO;
using System.Text;

namespace MesaPartesDigital
{
    /// <summary>
    /// Persistencia en archivo de texto (expedientes.txt), una línea por expediente:
    /// codigo|dni|nombre|asunto|fecha|estado
    /// Se eligió texto porque es legible, fácil de auditar y de probar.
    /// </summary>
    static class Persistencia
    {
        public static readonly string RUTA =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "expedientes.txt");

            public static bool Guardar()
        {
            try
            {
                string[] lineas = new string[Repositorio.cantidad];
                for (int i = 0; i < Repositorio.cantidad; i++)
                {
                    lineas[i] = Repositorio.codigos[i] + "|" + Repositorio.dnis[i] + "|" +
                                Repositorio.nombres[i] + "|" + Repositorio.asuntos[i] + "|" +
                                Repositorio.fechas[i] + "|" + Repositorio.estados[i];
                }
                File.WriteAllLines(RUTA, lineas, Encoding.UTF8);
                return true;
            }
            catch (IOException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
        }

            public static int Cargar()
        {
            Repositorio.cantidad = 0;

            if (!File.Exists(RUTA))
                return 0;

            try
            {
                string[] lineas = File.ReadAllLines(RUTA, Encoding.UTF8);
                for (int i = 0; i < lineas.Length; i++)
                {
                    if (lineas[i].Trim() == "")
                        continue;

                    string[] campos = lineas[i].Split('|');
                    if (campos.Length != 6 || Repositorio.cantidad >= Repositorio.CAPACIDAD)
                        continue; // línea dañada: se ignora

                    int p = Repositorio.cantidad;
                    Repositorio.codigos[p] = campos[0];
                    Repositorio.dnis[p] = campos[1];
                    Repositorio.nombres[p] = campos[2];
                    Repositorio.asuntos[p] = campos[3];
                    Repositorio.fechas[p] = campos[4];
                    Repositorio.estados[p] = campos[5];
                    Repositorio.cantidad++;
                }
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }

            return Repositorio.cantidad;
        }
    }
}
