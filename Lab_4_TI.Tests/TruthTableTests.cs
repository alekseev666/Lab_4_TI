using Lab_4_TI.Models;

namespace Lab_4_TI.Tests
{
    /// <summary>
    /// Тесты для таблицы истинности
    /// </summary>
    public class TruthTableTests
    {
        [Fact]
        public void IzNomera_ПростаяФункция_СоздаетТаблицу()
        {
            // Подготовка
            int n = 2;
            ulong nomer = 11; // 1011 в двоичной системе
            
            // Выполнение
            var tablitsa = TruthTable.IzNomera(n, nomer);
            
            // Проверка
            Assert.NotNull(tablitsa);
            Assert.Equal(4, tablitsa.Stroki.Count);
            Assert.Equal(2, tablitsa.Peremennie.Count);
        }

        [Fact]
        public void IzNomera_НулеваяФункция_ВсеНули()
        {
            // Данные
            int n = 2;
            ulong nomer = 0;
            
            // Действие
            var tablitsa = TruthTable.IzNomera(n, nomer);
            
            // Проверяем
            Assert.All(tablitsa.Rezultati, r => Assert.False(r));
        }

        [Fact]
        public void IzNomera_ОдинПеременная_ДваСтроки()
        {
            // Входные данные для теста
            int n = 1;
            ulong nomer = 2; // 10 в двочной системе
            
            // Запуск метода
            var tablitsa = TruthTable.IzNomera(n, nomer);
            
            // Проверка результата
            Assert.Equal(2, tablitsa.Stroki.Count);
            Assert.False(tablitsa.Rezultati[0]);
            Assert.True(tablitsa.Rezultati[1]);
        }

        [Fact]
        public void IzFunktsii_ПростоеИ_СоздаетПравильнуюТаблицу()
        {
            // Готовим токены для x1 & x2
            var lexer = new Lexer("x1 & x2");
            var tokeni = lexer.Tokenize().ToList();
            var opz = ParserRpn.VObramnuyuPolskuyuZapis(tokeni);
            var peremennie = Evaluator.PoluchitPeremennie(tokeni);
            
            // Строим таблицу
            var tablitsa = TruthTable.IzFunktsii(opz, peremennie);
            
            // Проверям что результаты правильные
            Assert.Equal(4, tablitsa.Rezultati.Count);
            Assert.False(tablitsa.Rezultati[0]); // 0 & 0 = 0
            Assert.False(tablitsa.Rezultati[1]); // 0 & 1 = 0
            Assert.False(tablitsa.Rezultati[2]); // 1 & 0 = 0
            Assert.True(tablitsa.Rezultati[3]);  // 1 & 1 = 1
        }
    }
}
