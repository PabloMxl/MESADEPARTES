using System;

namespace MesaPartesDigital
{
    static class Validacion
    {
            public static bool EsDniValido(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni) || dni.Length != 8)
                return false;

            for (int i = 0; i < dni.Length; i++)
            {
                if (!char.IsDigit(dni[i]))
                    return false;
            }
            return true;
        }

            public static bool EsNombreValido(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            nombre = nombre.Trim();
            if (nombre.Length < 3 || nombre.Length > 60)
                return false;

            for (int i = 0; i < nombre.Length; i++)
            {
                char c = nombre[i];
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            }
            return true;
        }

            public static bool EsAsuntoValido(string asunto)
        {
            if (string.IsNullOrWhiteSpace(asunto))
                return false;

            asunto = asunto.Trim();
            if (asunto.Length < 5 || asunto.Length > 200)
                return false;

            return asunto.IndexOf('|') < 0;
        }

            public static string Normalizar(string texto)
        {
            if (texto == null)
                return "";

            string[] partes = texto.Split(new char[] { ' ', '\t', '\r', '\n' },
                                          StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", partes);
        }

            public static string ValidarExpediente(string dni, string nombre, string asunto)
        {
            if (!EsDniValido(dni))
                return "El DNI debe tener exactamente 8 dígitos numéricos.";
            if (!EsNombreValido(nombre))
                return "El nombre debe tener entre 3 y 60 caracteres, solo letras y espacios.";
            if (!EsAsuntoValido(asunto))
                return "El asunto debe tener entre 5 y 200 caracteres y no puede contener el símbolo '|'.";
            return "";
        }
    }
}
