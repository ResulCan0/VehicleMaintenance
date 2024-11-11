using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("Log")]
public class LogController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public LogController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("Click")]
    public async Task<IActionResult> LogClick([FromBody] ClickLog model)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        string userIdString = userIdClaim?.Value; // Burada modelden string olarak alıyoruz

        Guid userId;
        bool isUserIdValid = Guid.TryParse(userIdString, out userId);

        // Tıklama logunu oluştur
        var clickLog = new ClickLog
        {
            ElementId = model.ElementId,
            ElementText = model.ElementText,
            Timestamp = model.Timestamp,
            UserId = isUserIdValid ? userId : Guid.NewGuid() // Geçerli kullanıcı kimliği veya yeni bir Guid
        };

        // Logu veritabanına kaydet
        _dbContext.ClickLogs.Add(clickLog);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }
}
