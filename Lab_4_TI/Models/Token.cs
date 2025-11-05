namespace Lab_4_TI.Models
{
    /// <summary>
    /// Перечисление типов токенов для парсинга логических выражений
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Переменая (например x1, x2)
        /// </summary>
        Peremennaya,
        
        /// <summary>
        /// Операция логическое отрицание (!)
        /// </summary>
        Ne,
        
        /// <summary>
        /// Операция логическое И (&)
        /// </summary>
        I,
        
        /// <summary>
        /// Операция логическе ИЛИ (|)
        /// </summary>
        Ili,
        
        /// <summary>
        /// Операция исключающего ИЛИ (^)
        /// </summary>
        Xor,
        
        /// <summary>
        /// Операция импликация (->)
        /// </summary>
        Implikatsiya,
        
        /// <summary>
        /// Операция эквивалентности (=)
        /// </summary>
        Ekvivalentnost,
        
        /// <summary>
        /// Левая скобка (
        /// </summary>
        LevaSkobka,
        
        /// <summary>
        /// Правая скобка )
        /// </summary>
        PravaSkobka
    }

    /// <summary>
    /// Представляет токен логического выражение
    /// </summary>
    /// <param name="Tip">Тип токэна</param>
    /// <param name="Tekst">Текстовое представление токена</param>
    public record Token(TokenType Tip, string Tekst);
}
