using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Lab_4_TI.Models
{
    /// <summary>
    /// Представляет таблицу истинности логической функции
    /// </summary>
    public class TruthTable
    {
        public List<string> Peremennie { get; }
        public List<bool[]> Stroki { get; }
        public List<bool> Rezultati { get; }

        public TruthTable(List<string> peremennie, List<bool[]> stroki, List<bool> rezultati)
        {
            Peremennie = peremennie;
            Stroki = stroki;
            Rezultati = rezultati;
        }

        /// <summary>
        /// Создает таблицу истинности по номеру функции (перегрузка для ulong)
        /// </summary>
        public static TruthTable IzNomera(int n, ulong nomer)
        {
            return IzNomera(n, new BigInteger(nomer));
        }

        /// <summary>
        /// Создает таблицу истинности по номеру функции (BigInteger)
        /// </summary>
        public static TruthTable IzNomera(int n, BigInteger nomer)
        {
            // Проверка диапазона n
            if (n < 1 || n > 20)
                throw new ArgumentException("n должен быть между 1 и 20");

            // Проверка границ без построения огромного "максимального" числа
            int dlinaVektora = 1 << n; // 2^n (для n<=20 помещается в int)
            if (nomer < BigInteger.Zero || (nomer >> dlinaVektora) != BigInteger.Zero)
                throw new ArgumentException($"Номер вне диапазона: допустимы значения от 0 до 2^(2^n)-1. Для n={n} допускается не более {dlinaVektora} бит в двоичном представлении номера.");

            // Создаем переменые x1, x2, x3...
            var peremennie = Enumerable.Range(1, n).Select(i => $"x{i}").ToList();
            var stroki = SgenerirovatVseStroki(n);
            var rezultati = new List<bool>();

            // Вычисляем результаты из номера функции
            for (int i = 0; i < stroki.Count; i++)
            {
                bool znachenie = ((nomer >> i) & BigInteger.One) == BigInteger.One;
                rezultati.Add(znachenie);
            }

            return new TruthTable(peremennie, stroki, rezultati);
        }

        /// <summary>
        /// Создает таблицу истинности по логическому выражению
        /// </summary>
        public static TruthTable IzFunktsii(List<Token> obramnayaZapis, List<string> peremennie)
        {
            // Генерируем все комбинации значений
            var stroki = SgenerirovatVseStroki(peremennie.Count);
            var rezultati = new List<bool>();

            // Вычисляем функцию для каждой строки
            foreach (var stroka in stroki)
            {
                var znacheniyaPeremennih = new Dictionary<string, bool>();
                for (int i = 0; i < peremennie.Count; i++)
                {
                    znacheniyaPeremennih[peremennie[i]] = stroka[i];
                }

                bool resultat = Evaluator.Vichislit(obramnayaZapis, znacheniyaPeremennih);
                rezultati.Add(resultat);
            }

            return new TruthTable(peremennie, stroki, rezultati);
        }

        /// <summary>
        /// Генерирует все возможные комбинации значений переменых
        /// </summary>
        private static List<bool[]> SgenerirovatVseStroki(int kolichestvoPeremennih)
        {
            int kolichestvoStrok = 1 << kolichestvoPeremennih;
            var stroki = new List<bool[]>();

            for (int i = 0; i < kolichestvoStrok; i++)
            {
                var stroka = new bool[kolichestvoPeremennih];
                for (int j = 0; j < kolichestvoPeremennih; j++)
                {
                    stroka[kolichestvoPeremennih - 1 - j] = ((i >> j) & 1) == 1;
                }
                stroki.Add(stroka);
            }

            return stroki;
        }

        /// <summary>
        /// Форматирует таблицу истинности в строку
        /// </summary>
        public static string TablitsaVStroku(TruthTable tablitsa, List<string> peremennie)
        {
            var sb = new StringBuilder();

            foreach (var peremennaya in peremennie)
            {
                sb.Append($"{peremennaya,-5}");
            }
            sb.AppendLine("f");

            for (int i = 0; i < tablitsa.Stroki.Count; i++)
            {
                foreach (var znachenie in tablitsa.Stroki[i])
                {
                    sb.Append($"{(znachenie ? "1" : "0"),-5}");
                }
                sb.AppendLine($"{(tablitsa.Rezultati[i] ? "1" : "0")}");
            }

            return sb.ToString();
        }
    }
}
