using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_4_TI.Models
{
    /// <summary>
    /// Генерирует СДНФ и СКНФ по таблице истинности
    /// </summary>
    public static class DnfKnfGenerator
    {
        /// <summary>
        /// Строит совершенную дизъюнктивную нормальную форму
        /// </summary>
        public static string VSDNF(TruthTable tablitsa, List<string> peremennie)
        {
            var termini = new List<string>();

            for (int i = 0; i < tablitsa.Stroki.Count; i++)
            {
                // Для СДНФ берем строки где результат = 1
                if (tablitsa.Rezultati[i])
                {
                    var literali = new List<string>();
                    for (int j = 0; j < peremennie.Count; j++)
                    {
                        string literal = tablitsa.Stroki[i][j] ? peremennie[j] : $"!{peremennie[j]}";
                        literali.Add(literal);
                    }
                    termini.Add($"({string.Join(" & ", literali)})");
                }
            }

            return termini.Count == 0 ? "0" : string.Join(" | ", termini);
        }

        /// <summary>
        /// Строит совершенную конъюнктивную нормальную форму
        /// </summary>
        public static string VSKNF(TruthTable tablitsa, List<string> peremennie)
        {
            var klauzi = new List<string>();

            // Проходим по таблице
            for (int i = 0; i < tablitsa.Stroki.Count; i++)
            {
                // Для СКНФ берем строки где результат = 0
                if (!tablitsa.Rezultati[i])
                {
                    var literali = new List<string>();
                    for (int j = 0; j < peremennie.Count; j++)
                    {
                        string literal = tablitsa.Stroki[i][j] ? $"!{peremennie[j]}" : peremennie[j];
                        literali.Add(literal);
                    }
                    klauzi.Add($"({string.Join(" | ", literali)})");
                }
            }

            return klauzi.Count == 0 ? "1" : string.Join(" & ", klauzi);
        }

        /// <summary>
        /// Подсчитывает стоимость СДНФ
        /// </summary>
        public static (int literali, int konYunktsii, int disYunktsii) PodschitatSDNF(string sdnf)
        {
            if (sdnf == "0") return (0, 0, 0);

            var termini = sdnf.Split(" | ", StringSplitOptions.RemoveEmptyEntries);
            int konYunktsii = termini.Length;
            int disYunktsii = Math.Max(0, termini.Length - 1);

            // Подсчет литералов
            int literali = 0;
            foreach (var termin in termini)
            {
                string ochishenniyTermin = termin.Trim('(', ')');
                var literaliTermina = ochishenniyTermin.Split(" & ", StringSplitOptions.RemoveEmptyEntries);
                literali += literaliTermina.Length;
            }

            return (literali, konYunktsii, disYunktsii);
        }

        /// <summary>
        /// Подсчитывает стоимость СКНФ
        /// </summary>
        public static (int literali, int konYunktsii, int disYunktsii) PodschitatSKNF(string sknf)
        {
            if (sknf == "1") return (0, 0, 0);

            var klauzi = sknf.Split(" & ", StringSplitOptions.RemoveEmptyEntries);
            int disYunktsii = klauzi.Length;
            int konYunktsii = Math.Max(0, klauzi.Length - 1);

            int literali = 0;
            foreach (var klauza in klauzi)
            {
                string ochishennayaKlauza = klauza.Trim('(', ')');
                var literaliKlauzi = ochishennayaKlauza.Split(" | ", StringSplitOptions.RemoveEmptyEntries);
                literali += literaliKlauzi.Length;
            }

            return (literali, konYunktsii, disYunktsii);
        }
    }
}