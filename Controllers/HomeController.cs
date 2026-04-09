using System.Diagnostics;
using Hydrox.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace Hydrox.Controllers
{
    /// <summary>
    /// Home Controller for handling requests related to the home page and other static pages.
    /// </summary>
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
        public async Task<IActionResult> SendEmailByGmail([FromBody] ContactForm model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Message))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                using (var mailMessage = new MailMessage())
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("chaturvedimanish100780@gmail.com", "mtgw tjlx utek gnri");
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Timeout = 10000;

                    mailMessage.From = new MailAddress("chaturvedimanish100780@gmail.com");
                    mailMessage.To.Add("chaturvedimanish100780@gmail.com");
                    mailMessage.Subject = "New Contact Form Submission for Hydrox";
                    mailMessage.Body = $"Name: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}";
                    mailMessage.IsBodyHtml = false;

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return Ok(new { message = "Message sent successfully!" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error sending message.", error = ex.ToString() });
            }
        }


        public async Task<IActionResult> SendEmail([FromBody] ContactForm model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Message))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            try
            {
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")?.Trim();
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new Exception("SendGrid API key missing or invalid");
                }

                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("chaturvedimanish100780@gmail.com", "Hydrox Website");
                var to = new EmailAddress("info@hydrox.co.in");

                var subject = "New Contact Form Submission for Hydrox";

                var plainTextContent =
                    $"Name: {model.Name}\nEmail: {model.Email}\n\nMessage:\n{model.Message}";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, null);

                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    var errorBody = await response.Body.ReadAsStringAsync();

                    return StatusCode(500, new
                    {
                        message = "SendGrid error",
                        error = errorBody
                    });
                }

                return Ok(new { message = "Message sent successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error sending message.",
                    error = ex.ToString()
                });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = exceptionFeature?.Error?.ToString()
            };

            return View(model);

        }
    }
}
