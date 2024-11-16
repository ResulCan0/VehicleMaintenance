public class FeaturedNumber
{
    public int Id { get; set; } // Benzersiz kimlik
    public string Keys { get; set; } // Dinamik alanın anahtarı (ör. "featured-number")
    public string Value { get; set; } // Dinamik alanın değeri (ör. "20+")
}