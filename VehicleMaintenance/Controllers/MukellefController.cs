using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace VehicleMaintenance.Controllers
{
    public class MukellefController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MukellefController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // CAPTCHA resmini al
            var captchaImageUrl = "https://sorgu.efatura.gov.tr/kullanicilar/img.php";
            var captchaImage = await GetCaptchaImageAsync(captchaImageUrl);
            if (captchaImage != null)
            {
                // CAPTCHA resmini geçici bir dosyaya kaydet
                var captchaImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "captcha", "captcha.jpg");
                string directoryPath = Path.GetDirectoryName(captchaImagePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                System.IO.File.WriteAllBytes(captchaImagePath, captchaImage);

                ViewBag.CaptchaImagePath = "/captcha/captcha.jpg"; // Görüntüleme için yol
            }
            else
            {
                ViewBag.Error = "Captcha resmini alırken bir hata oluştu.";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Sorgula(string vergiNo, string captchaCode)
        {
            if (string.IsNullOrEmpty(vergiNo) || string.IsNullOrEmpty(captchaCode))
            {
                ViewBag.Message = "Lütfen tüm alanları doldurun.";
                return View("Index");
            }

            var result = await SorgulaMukellefAsync(vergiNo, captchaCode);
            ViewBag.SorguSonucu = result;
            return View("Index");
        }

        private async Task<byte[]> GetCaptchaImageAsync(string captchaUrl)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Referer", "https://sorgu.efatura.gov.tr/");
            client.Timeout = TimeSpan.FromSeconds(30); // 30 saniye zaman aşımı
            var response = await client.GetAsync(captchaUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }

        private async Task<string> SorgulaMukellefAsync(string vergiNo, string captchaCode)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Referer", "https://sorgu.efatura.gov.tr/");
            client.Timeout = TimeSpan.FromSeconds(30); // 30 saniye zaman aşımı
            var postData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("search_string", vergiNo),
                new KeyValuePair<string, string>("captcha_code", captchaCode),
                new KeyValuePair<string, string>("submit", "Ara")
            });

            var response = await client.PostAsync("https://sorgu.efatura.gov.tr/kullanicilar/xliste.php", postData);

            if (!response.IsSuccessStatusCode)
                return "Mükellef bilgisi alınamadı.";

            var responseContent = await response.Content.ReadAsStringAsync();

            if (responseContent.Contains("Güvenlik kodu hatalı"))
                return "Captcha hatalı, lütfen tekrar deneyin.";

            return responseContent.Contains("Mükellef kayıtlıdır")
                ? "Mükellef kayıtlıdır."
                : "Mükellef kaydı bulunamadı.";

        }

    }
}
