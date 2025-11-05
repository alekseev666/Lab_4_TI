using Lab_4_TI.Models;

namespace Lab_4_TI.Tests
{
    /// <summary>
    /// Тесты лексического анализатора
    /// </summary>
    public class LexerTests
    {
        [Fact]
        public void Tokenize_ПростоеВыражение_ВозвращаетТокены()
        {
            // Подготовка
            var lexer = new Lexer("x1 & x2");
            
            // Действие
            var tokeni = lexer.Tokenize().ToList();
            
            // Проверка
            Assert.Equal(3, tokeni.Count);
            Assert.Equal(TokenType.Peremennaya, tokeni[0].Tip);
            Assert.Equal(TokenType.I, tokeni[1].Tip);
            Assert.Equal(TokenType.Peremennaya, tokeni[2].Tip);
        }

        [Fact]
        public void Tokenize_СоСкобками_ОбрабатываетПравильно()
        {
            // Подготовка данных
            var lexer = new Lexer("(x1 | x2)");
            
            // Выполнение
            var tokeni = lexer.Tokenize().ToList();
            
            // Проверка результатов
            Assert.Equal(5, tokeni.Count);
            Assert.Equal(TokenType.LevaSkobka, tokeni[0].Tip);
            Assert.Equal(TokenType.PravaSkobka, tokeni[4].Tip);
        }

        [Fact]
        public void Tokenize_Отрицание_СоздаетТокенНе()
        {
            // Даные для теста
            var lexer = new Lexer("!x1");
            
            // Запуск
            var tokeni = lexer.Tokenize().ToList();
            
            // Проверяем
            Assert.Equal(2, tokeni.Count);
            Assert.Equal(TokenType.Ne, tokeni[0].Tip);
        }

        [Fact]
        public void Tokenize_Импликация_РаспознаетОператор()
        {
            // Входные данные
            var lexer = new Lexer("x1 -> x2");
            
            // Вычисление
            var tokeni = lexer.Tokenize().ToList();
            
            // Утверждение
            Assert.Equal(3, tokeni.Count);
            Assert.Equal(TokenType.Implikatsiya, tokeni[1].Tip);
        }
    }
}
