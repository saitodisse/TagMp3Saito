using System;
using System.Text.RegularExpressions;

namespace TagMp3Saito_WindowsFormsApplication
{
    public static class StringTools
    {
        public static string Get_Date_Hour()
        {
            string texto = DateTime.Now.ToString();

            string dia, mes, ano, hora, minuto, segundo;
            Regex reg;
            string pat =
                @"(0?[1-9]|[12][0-9]|3[01])\/(0?[1-9]|1[0-2])\/(19[0-9]{2}|2[0-9]{3}|\d{2})\s?(\d+)?:?(\d+)?:?(\d+)?";
            reg = new Regex(pat);
            Match m = reg.Match(texto);

            if (m.Length == 0)
                return string.Empty;


            dia = (m.Groups[1].Value.Length == 1) ? "0" + m.Groups[1].Value : m.Groups[1].Value;
            mes = (m.Groups[2].Value.Length == 1) ? "0" + m.Groups[2].Value : m.Groups[2].Value;
            ano = m.Groups[3].Value;
            hora = m.Groups[4].Value;
            minuto = m.Groups[5].Value;
            segundo = m.Groups[6].Value;

            return String.Format("{0}_{1}_{2}_{3}_{4}_{5}",
                                 ano,
                                 mes,
                                 dia,
                                 hora,
                                 minuto,
                                 segundo);
        }
    }
}