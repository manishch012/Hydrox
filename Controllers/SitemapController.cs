using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Hydrox.Controllers
{
    [Route("sitemap.xml")]
    public class SitemapController : Controller
    {
        public IActionResult Index()
        {
            var sitemap = GenerateSitemap();
            var xmlSettings = new XmlWriterSettings { OmitXmlDeclaration = true };
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))
            {
                sitemap.WriteTo(xmlWriter);
                xmlWriter.Flush();
                return Content(stringWriter.GetStringBuilder().ToString(), "application/xml", Encoding.UTF8);
            }
        }

        private XElement GenerateSitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var urls = new[]
            {
                new { Url = "https://www.hydrox.co.in", LastModified = "2025-03-22", ChangeFrequency = "daily", Priority = 1.0 },
                new { Url = "https://www.hydrox.co.in/Home/Index", LastModified = "2025-03-22", ChangeFrequency = "daily", Priority = 1.0 },
                new { Url = "https://www.hydrox.co.in/Home/client", LastModified = "2025-03-22", ChangeFrequency = "daily", Priority = 1.0 },
                new { Url = "https://www.hydrox.co.in/Home/partner", LastModified = "2025-03-22", ChangeFrequency = "daily", Priority = 1.0 },
                new { Url = "https://www.hydrox.co.in/Home/catalog", LastModified = "2025-03-22", ChangeFrequency = "daily", Priority = 1.0 },
                new { Url = "https://www.hydrox.co.in/Home/contactus", LastModified = "2025-03-22", ChangeFrequency = "daily", Priority = 1.0 },
                new { Url = "https://www.hydrox.co.in/Home/services", LastModified = "2025-03-22", ChangeFrequency = "daily", Priority = 1.0 },
                new { Url = "https://www.hydrox.co.in/Gallery/index", LastModified = "2025-03-22", ChangeFrequency = "daily", Priority = 1.0 }
                // Add other pages of your website here
            };

            var urlSet = new XElement(ns + "urlset");

            foreach (var url in urls)
            {
                var urlElement = new XElement(ns + "url",
                    new XElement(ns + "loc", url.Url),
                    new XElement(ns + "lastmod", url.LastModified),
                    new XElement(ns + "changefreq", url.ChangeFrequency),
                    new XElement(ns + "priority", url.Priority));

                urlSet.Add(urlElement);
            }

            return urlSet;
        }
    }
}
