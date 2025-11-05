
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_4_TI.Models
{
    /// <summary>
    /// Разбивает строку на лексэмы для дальнейшей обработки
    /// </summary>
    public class Lexer
    {
        private readonly string _vhod;
        private int _pozitsiya;

        public Lexer(string vhod)
        {
            _vhod = vhod ?? string.Empty;
            _pozitsiya = 0;
        }

        /// <summary>
        /// Разбивает входную строку на последовательность токэнов
        /// </summary>
        public IEnumerable<Token> Tokenize()
        {
            // Проходим по всей строке
            while (_pozitsiya < _vhod.Length)
            {
                char tekushiy = _vhod[_pozitsiya];

                // Пропускаем пробелы
                if (char.IsWhiteSpace(tekushiy))
                {
                    _pozitsiya++;
                    continue;
                }

                // Если буква - это идентификатор
                if (char.IsLetter(tekushiy))
                {
                    string identifikator = ProchitatIdentifikator();
                    yield return IdentifikatorVToken(identifikator);
                    continue;
                }

                // Обрабатываем операторы
                switch (tekushiy)
                {
                    case '(':
                        _pozitsiya++;
                        yield return new Token(TokenType.LevaSkobka, "(");
                        break;
                    case ')':
                        _pozitsiya++;
                        yield return new Token(TokenType.PravaSkobka, ")");
                        break;
                    case '!':
                        _pozitsiya++;
                        yield return new Token(TokenType.Ne, "!");
                        break;
                    case '&':
                        _pozitsiya++;
                        yield return new Token(TokenType.I, "&");
                        break;
                    case '|':
                        _pozitsiya++;
                        yield return new Token(TokenType.Ili, "|");
                        break;
                    case '^':
                        _pozitsiya++;
                        yield return new Token(TokenType.Xor, "^");
                        break;
                    case '-':
                        if (_pozitsiya + 1 < _vhod.Length && _vhod[_pozitsiya + 1] == '>')
                        {
                            _pozitsiya += 2;
                            yield return new Token(TokenType.Implikatsiya, "->");
                        }
                        else
                        {
                            throw new Exception($"Неожидаемый символ '{tekushiy}' на позиции {_pozitsiya}");
                        }
                        break;
                    case '=':
                        _pozitsiya++;
                        yield return new Token(TokenType.Ekvivalentnost, "=");
                        break;
                    default:
                        throw new Exception($"Неожидаемый символ '{tekushiy}' на позиции {_pozitsiya}");
                }
            }
        }

        /// <summary>
        /// Читает идентификатор (буквы, цифры, подчеркивание)
        /// </summary>
        private string ProchitatIdentifikator()
        {
            var resultat = new StringBuilder();
            while (_pozitsiya < _vhod.Length && (char.IsLetterOrDigit(_vhod[_pozitsiya]) || _vhod[_pozitsiya] == '_'))
            {
                resultat.Append(_vhod[_pozitsiya]);
                _pozitsiya++;
            }
            return resultat.ToString();
        }

        /// <summary>
        /// Преобразует идентификатор в токен
        /// </summary>
        private Token IdentifikatorVToken(string identifikator)
        {
            return identifikator.ToLower() switch
            {
                "not" or "!" => new Token(TokenType.Ne, identifikator),
                "and" or "&" => new Token(TokenType.I, identifikator),
                "or" or "|" => new Token(TokenType.Ili, identifikator),
                "xor" or "^" => new Token(TokenType.Xor, identifikator),
                "->" => new Token(TokenType.Implikatsiya, identifikator),
                "=" => new Token(TokenType.Ekvivalentnost, identifikator),
                _ => new Token(TokenType.Peremennaya, identifikator)
            };
        }
    }
}