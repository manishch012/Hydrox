using System.Diagnostics;
using Hydrox.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;


namespace Hydrox.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/catalog");

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Client()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Partner()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult WhyUs()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetImages()
        {
            string imagesPath = Path.Combine(_env.WebRootPath, "Images/catalog");

            if (!Directory.Exists(imagesPath))
            {
                return NotFound("Images folder not found.");
            }

            var images = Directory.GetFiles(_imagesPath)
                                  .Select(Path.GetFileName)
                                  .ToList();

            return Ok(images);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] ContactForm model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Message))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("chaturvedimanish100780@gmail.com", "pxlerslrpbnxdjuw"), // Use App Password
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("chaturvedimanish100780@gmail.com"),
                    Subject = "New Contact Form Submission",
                    Body = $"Name: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}",
                    IsBodyHtml = false
                };
                mailMessage.To.Add("info@hydrox.co.in"); // Receiver email

                await smtpClient.SendMailAsync(mailMessage);
                return Ok(new { message = "Message sent successfully!" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error sending message.", error = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
