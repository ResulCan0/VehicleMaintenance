using System;

public static class DynamicContentHelper
{
    private static IServiceProvider _serviceProvider;

    // Servis sağlayıcısını al
    public static void Configure(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // Key'e göre değeri getir
    public static string Value(string key)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var entry = dbContext.FeaturedNumbers.FirstOrDefault(x => x.Keys == key);
            return entry?.Value ?? "0"; // Varsayılan değer 0
        }
    }
}
