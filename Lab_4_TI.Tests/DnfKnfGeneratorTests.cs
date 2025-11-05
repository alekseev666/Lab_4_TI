using Lab_4_TI.Models;

namespace Lab_4_TI.Tests
{
    /// <summary>
    /// Тесты для генератора нормальных форм
    /// </summary>
    public class DnfKnfGeneratorTests
    {
        [Fact]
        public void VSDNF_ПростаяФункция_ГенерируетСДНФ()
        {
            // Подготовка
            var tablitsa = TruthTable.IzNomera(2, 11);
            var peremennie = new List<string> { "x1", "x2" };
            
            // Выполнение
            var sdnf = DnfKnfGenerator.VSDNF(tablitsa, peremennie);
            
            // Проверка
            Assert.NotNull(sdnf);
            Assert.NotEmpty(sdnf);
            Assert.Contains("&", sdnf);
            Assert.Contains("|", sdnf);
        }

        [Fact]
        public void VSKNF_ПростаяФункция_ГенерируетСКНФ()
        {
            // Данные для теста
            var tablitsa = TruthTable.IzNomera(2, 11);
            var peremennie = new List<string> { "x1", "x2" };
            
            // Запуск
            var sknf = DnfKnfGenerator.VSKNF(tablitsa, peremennie);
            
            // Провермя результат
            Assert.NotNull(sknf);
            Assert.NotEmpty(sknf);
        }

        [Fact]
        public void PodschitatSDNF_ПростоеВыражение_ПодсчитываетЭлементы()
        {
            // Входное выражение
            string sdnf = "(x1 & !x2) | (!x1 & x2)";
            
            // Подсчитываем
            var (literali, konYunktsii, disYunktsii) = DnfKnfGenerator.PodschitatSDNF(sdnf);
            
            // Проверка
            Assert.Equal(4, literali);
            Assert.Equal(2, konYunktsii);
            Assert.Equal(1, disYunktsii);
        }

        [Fact]
        public void VSDNF_НулеваяФункция_ВозвращаетНоль()
        {
            // Настройка теста
            var tablitsa = TruthTable.IzNomera(2, 0);
            var peremennie = new List<string> { "x1", "x2" };
            
            // Действие
            var sdnf = DnfKnfGenerator.VSDNF(tablitsa, peremennie);
            
            // Проверяем
            Assert.Equal("0", sdnf);
        }

        [Fact]
        public void VSKNF_ЕдиничнаяФункция_ВозвращаетЕдиницу()
        {
            // Подготовка данных
            var tablitsa = TruthTable.IzNomera(2, 15); // все единицы
            var peremennie = new List<string> { "x1", "x2" };
            
            // Вычисляем СКНФ
            var sknf = DnfKnfGenerator.VSKNF(tablitsa, peremennie);
            
            // Утверждаем
            Assert.Equal("1", sknf);
        }
    }
}
