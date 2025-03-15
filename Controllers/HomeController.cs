using System.Diagnostics;
//using System.Net.Mail;
using Hydrox.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;


namespace Hydrox.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/catalog");

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
            string imagesPath = Path.Combine(_env.WebRootPath, "images/catalog");

            if (!Directory.Exists(imagesPath))
            {
                return NotFound("Images folder not found.");
            }

            var images = Directory.GetFiles(_imagesPath)
                                  .Select(Path.GetFileName)
                                  .ToList();

            return Ok(images);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromForm] ContactForm model)
        {
            if (ModelState.IsValid)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Your Website", "your-email@example.com"));
                emailMessage.To.Add(new MailboxAddress("Admin", "admin@example.com"));
                emailMessage.Subject = "New Contact Form Submission";
                emailMessage.Body = new TextPart("plain")
                {
                    Text = $"Name: {model.Name}\nEmail: {model.Email}\nMessage:\n{model.Message}"
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.example.com", 587, false); // Use your SMTP server
                    await client.AuthenticateAsync("your-email@example.com", "your-email-password");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                return Ok(new { message = "Email sent successfully!" });
            }

            return BadRequest(ModelState);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
