namespace Lab_4_TI.Models
{
    public enum TokenType
    {
        Peremennaya,
        Ne,
        I,
        Ili,
        Xor,
        Implikatsiya,
        Ekvivalentnost,
        LevaSkobka,
        PravaSkobka
    }

    public record Token(TokenType Tip, string Tekst);
}