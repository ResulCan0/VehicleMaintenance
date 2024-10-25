using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        // Veritabanı context'ini isteğe bağlı olarak alın (scoped yaşam döngüsü içinde)
        var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();
        var userIdString = context.Session.GetString("UserId");
        Guid userId;
        bool isUserIdValid = Guid.TryParse(userIdString, out userId);
      
        var log = new Log
        {
            UserId = isUserIdValid ? userId : Guid.NewGuid(),
            Path = request.Path,
            Method = request.Method,
            QueryString = request.QueryString.ToString(),
            RequestBody = await ReadRequestBodyAsync(request),
            Headers = string.Join("; ", request.Headers.Select(h => $"{h.Key}: {h.Value}")),
            Timestamp = DateTime.Now
        };

        // Logu veritabanına ekle
        dbContext.Logging.Add(log);
        await dbContext.SaveChangesAsync();

      
        await _next(context);
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        // Sadece POST ve PUT gibi gövdesi olan isteklerde gövdeyi oku
        if (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put)
        {
            request.EnableBuffering(); 
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0; 
            return body;
        }
        return null;
    }
}
