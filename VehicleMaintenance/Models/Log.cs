public class Log
{
    public Guid Id { get; set; } = new Guid(); // Otomatik artan birincil anahtar
    public Guid UserId { get; set; } // Kullanıcı kimliği
    public string Path { get; set; } // Talep edilen sayfa yolu
    public string Method { get; set; } // HTTP metodu (GET, POST vs.)
    public string QueryString { get; set; } // Query string bilgisi
    public string? RequestBody { get; set; } // İstek gövdesi (POST dataları vb.)
    public DateTime Timestamp { get; set; } // Talep zamanı
    public string Headers { get; set; } // İstek başlıkları
}
public class ClickLog
{
    public int Id { get; set; } // Id özelliği
    public string ElementId { get; set; }
    public string ElementText { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid? UserId { get; set; } // Kullanıcı kimliğini tutacak özellik
}

