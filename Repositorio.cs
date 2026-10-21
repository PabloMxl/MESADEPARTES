using System;

namespace MesaPartesDigital
{
    // Usamos arreglos paralelos: la misma posición pertenece al mismo expediente.
    static class Repositorio
    {
        public const int CAPACIDAD = 1000;

        public static readonly string[] ESTADOS = { "Recibido", "En proceso", "Atendido" };

        public static string[] codigos = new string[CAPACIDAD];
        public static string[] dnis = new string[CAPACIDAD];
        public static string[] nombres = new string[CAPACIDAD];
        public static string[] asuntos = new string[CAPACIDAD];
        public static string[] fechas = new string[CAPACIDAD];
        public static string[] estados = new string[CAPACIDAD];
        public static int cantidad = 0;

            // Busca el número mayor para generar el siguiente código.
        public static string GenerarCodigo()
        {
            string prefijo = "EXP-" + DateTime.Now.Year + "-";
            int mayor = 0;

            for (int i = 0; i < cantidad; i++)
            {
                if (codigos[i].StartsWith(prefijo))
                {
                    int numero;
                    if (int.TryParse(codigos[i].Substring(prefijo.Length), out numero) && numero > mayor)
                        mayor = numero;
                }
            }
            return prefijo + (mayor + 1).ToString("D4");
        }

            public static string Registrar(string dni, string nombre, string asunto, string estado)
        {
            if (cantidad >= CAPACIDAD)
                return "";

            string codigo = GenerarCodigo();
            codigos[cantidad] = codigo;
            dnis[cantidad] = dni;
            nombres[cantidad] = nombre;
            asuntos[cantidad] = asunto;
            fechas[cantidad] = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            estados[cantidad] = estado;
            cantidad++;
            return codigo;
        }

            public static int BuscarPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return -1;

            codigo = codigo.Trim().ToUpper();
            for (int i = 0; i < cantidad; i++)
            {
                if (codigos[i].ToUpper() == codigo)
                    return i;
            }
            return -1;
        }

            // Ordenamiento por selección, comparando los códigos.
        public static void OrdenarPorCodigo()
        {
            for (int i = 0; i < cantidad - 1; i++)
            {
                int menor = i;
                for (int j = i + 1; j < cantidad; j++)
                {
                    if (string.CompareOrdinal(codigos[j], codigos[menor]) < 0)
                        menor = j;
                }
                if (menor != i)
                    Intercambiar(i, menor);
            }
        }

            private static void Intercambiar(int a, int b)
        {
            Intercambiar(codigos, a, b);
            Intercambiar(dnis, a, b);
            Intercambiar(nombres, a, b);
            Intercambiar(asuntos, a, b);
            Intercambiar(fechas, a, b);
            Intercambiar(estados, a, b);
        }

        private static void Intercambiar(string[] arreglo, int a, int b)
        {
            string temporal = arreglo[a];
            arreglo[a] = arreglo[b];
            arreglo[b] = temporal;
        }

            public static bool ActualizarEstado(int indice, string nuevoEstado)
        {
            if (indice < 0 || indice >= cantidad)
                return false;
            estados[indice] = nuevoEstado;
            return true;
        }
    }
}
