
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_4_TI.Models
{
    /// <summary>
    /// Вычисляет значение логического выражения в ОПЗ
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Вычисляет значение выражения для заданных значений переменных
        /// </summary>
        public static bool Vichislit(List<Token> obramnayaZapis, Dictionary<string, bool> znacheniyaPeremennih)
        {
            // Стек для вычисления ОПЗ
            var stek = new Stack<bool>();

            // Проходим по токенам
            foreach (var token in obramnayaZapis)
            {
                switch (token.Tip)
                {
                    case TokenType.Peremennaya:
                        if (!znacheniyaPeremennih.ContainsKey(token.Tekst))
                            throw new Exception($"Неопределенная переменная: {token.Tekst}");
                        stek.Push(znacheniyaPeremennih[token.Tekst]);
                        break;

                    case TokenType.Ne:
                        if (stek.Count < 1) throw new Exception("Некорректное выражение - недостаточно операндов для !");
                        stek.Push(!stek.Pop());
                        break;

                    case TokenType.I:
                        if (stek.Count < 2) throw new Exception("Некорректное выражение - недостаточно операндов для &");
                        var praviy1 = stek.Pop();
                        var leviy1 = stek.Pop();
                        stek.Push(leviy1 && praviy1);
                        break;

                    case TokenType.Ili:
                        if (stek.Count < 2) throw new Exception("Некорректное выражение - недостаточно операндов для |");
                        var praviy2 = stek.Pop();
                        var leviy2 = stek.Pop();
                        stek.Push(leviy2 || praviy2);
                        break;

                    default:
                        throw new Exception($"Неожидаемый токэн в ОПЗ: {token.Tip}");
                }
            }

            if (stek.Count != 1)
                throw new Exception($"Некорректное выражение - осталось {stek.Count} элементов в стеке вместо 1");

            return stek.Pop();
        }

        /// <summary>
        /// Извлекает список уникальных переменных из выражения
        /// </summary>
        public static List<string> PoluchitPeremennie(List<Token> tokeni)
        {
            return tokeni
                .Where(t => t.Tip == TokenType.Peremennaya)
                .Select(t => t.Tekst)
                .Distinct()
                .OrderBy(v => v)
                .ToList();
        }
    }
}