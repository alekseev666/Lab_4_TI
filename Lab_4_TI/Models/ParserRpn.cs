
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_4_TI.Models
{
    /// <summary>
    /// Преобразует последовательность токэнов в обратную польскую запись
    /// </summary>
    public static class ParserRpn
    {
        private static int PoluchitPrioritet(TokenType tipTokena)
        {
            return tipTokena switch
            {
                TokenType.Ne => 5,
                TokenType.I => 4,
                TokenType.Ili => 3,
                TokenType.Xor => 2,
                TokenType.Implikatsiya => 1,
                TokenType.Ekvivalentnost => 0,
                _ => -1
            };
        }

        /// <summary>
        /// Конвертирует токэны в ОПЗ с разворотом в базис
        /// </summary>
        public static List<Token> VObramnuyuPolskuyuZapis(IEnumerable<Token> tokeni)
        {
            var vihod = new List<Token>();
            var operatori = new Stack<Token>();

            foreach (var token in tokeni)
            {
                switch (token.Tip)
                {
                    case TokenType.Peremennaya:
                        vihod.Add(token);
                        break;

                    case TokenType.LevaSkobka:
                        operatori.Push(token);
                        break;

                    case TokenType.PravaSkobka:
                        while (operatori.Count > 0 && operatori.Peek().Tip != TokenType.LevaSkobka)
                        {
                            vihod.Add(operatori.Pop());
                        }
                        if (operatori.Count == 0 || operatori.Peek().Tip != TokenType.LevaSkobka)
                            throw new Exception("Несбалансированые скобки");
                        operatori.Pop();
                        break;

                    case TokenType.Ne:
                    case TokenType.I:
                    case TokenType.Ili:
                    case TokenType.Xor:
                    case TokenType.Implikatsiya:
                    case TokenType.Ekvivalentnost:
                        while (operatori.Count > 0 && operatori.Peek().Tip != TokenType.LevaSkobka &&
                               PoluchitPrioritet(operatori.Peek().Tip) >= PoluchitPrioritet(token.Tip))
                        {
                            vihod.Add(operatori.Pop());
                        }
                        operatori.Push(token);
                        break;
                }
            }

            while (operatori.Count > 0)
            {
                var op = operatori.Pop();
                if (op.Tip == TokenType.LevaSkobka || op.Tip == TokenType.PravaSkobka)
                    throw new Exception("Несбалансированые скобки");
                vihod.Add(op);
            }

            return RazvernutVBazise(vihod);
        }

        private static List<Token> RazvernutVBazise(List<Token> obramnayaZapis)
        {
            var stek = new Stack<List<Token>>();

            foreach (var token in obramnayaZapis)
            {
                if (token.Tip == TokenType.Peremennaya)
                {
                    stek.Push(new List<Token> { token });
                }
                else if (token.Tip == TokenType.Ne)
                {
                    var operand = stek.Pop();
                    operand.Add(token);
                    stek.Push(operand);
                }
                else
                {
                    var praviy = stek.Pop();
                    var leviy = stek.Pop();

                    var novoeVirazhenie = token.Tip switch
                    {
                        TokenType.I => SozdatI_Virazhenie(leviy, praviy),
                        TokenType.Ili => SozdatIli_Virazhenie(leviy, praviy),
                        TokenType.Xor => SozdatXorVirazhenie(leviy, praviy),
                        TokenType.Implikatsiya => SozdatImplikatsiyu(leviy, praviy),
                        TokenType.Ekvivalentnost => SozdatEkvivalentnost(leviy, praviy),
                        _ => throw new Exception($"Неподдерживаемая операция: {token.Tip}")
                    };

                    stek.Push(novoeVirazhenie);
                }
            }

            if (stek.Count != 1)
                throw new Exception("Некорректная ОПЗ выражение");

            return stek.Pop();
        }

        private static List<Token> SozdatI_Virazhenie(List<Token> leviy, List<Token> praviy)
        {
            var resultat = new List<Token>();
            resultat.AddRange(leviy);
            resultat.AddRange(praviy);
            resultat.Add(new Token(TokenType.I, "&"));
            return resultat;
        }

        private static List<Token> SozdatIli_Virazhenie(List<Token> leviy, List<Token> praviy)
        {
            var resultat = new List<Token>();
            resultat.AddRange(leviy);
            resultat.AddRange(praviy);
            resultat.Add(new Token(TokenType.Ili, "|"));
            return resultat;
        }

        private static List<Token> SozdatXorVirazhenie(List<Token> leviy, List<Token> praviy)
        {
            var neLeviy = new List<Token>(leviy) { new Token(TokenType.Ne, "!") };
            var nePraviy = new List<Token>(praviy) { new Token(TokenType.Ne, "!") };

            var leviyINePraviy = new List<Token>();
            leviyINePraviy.AddRange(leviy);
            leviyINePraviy.AddRange(nePraviy);
            leviyINePraviy.Add(new Token(TokenType.I, "&"));

            var neLeviyIPraviy = new List<Token>();
            neLeviyIPraviy.AddRange(neLeviy);
            neLeviyIPraviy.AddRange(praviy);
            neLeviyIPraviy.Add(new Token(TokenType.I, "&"));

            var resultat = new List<Token>();
            resultat.AddRange(leviyINePraviy);
            resultat.AddRange(neLeviyIPraviy);
            resultat.Add(new Token(TokenType.Ili, "|"));

            return resultat;
        }

        private static List<Token> SozdatImplikatsiyu(List<Token> leviy, List<Token> praviy)
        {
            var neLeviy = new List<Token>(leviy) { new Token(TokenType.Ne, "!") };

            var resultat = new List<Token>();
            resultat.AddRange(neLeviy);
            resultat.AddRange(praviy);
            resultat.Add(new Token(TokenType.Ili, "|"));

            return resultat;
        }

        private static List<Token> SozdatEkvivalentnost(List<Token> leviy, List<Token> praviy)
        {
            var neLeviy = new List<Token>(leviy) { new Token(TokenType.Ne, "!") };
            var nePraviy = new List<Token>(praviy) { new Token(TokenType.Ne, "!") };

            var leviyIPraviy = new List<Token>();
            leviyIPraviy.AddRange(leviy);
            leviyIPraviy.AddRange(praviy);
            leviyIPraviy.Add(new Token(TokenType.I, "&"));

            var neLeviyINePraviy = new List<Token>();
            neLeviyINePraviy.AddRange(neLeviy);
            neLeviyINePraviy.AddRange(nePraviy);
            neLeviyINePraviy.Add(new Token(TokenType.I, "&"));

            var resultat = new List<Token>();
            resultat.AddRange(leviyIPraviy);
            resultat.AddRange(neLeviyINePraviy);
            resultat.Add(new Token(TokenType.Ili, "|"));

            return resultat;
        }
    }
}