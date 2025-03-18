using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hydrox.Models;

namespace Hydrox.Controllers
{
    public class GalleryController : Controller
    {
        private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Gallery");
        private const int PageSize = 6; // Number of images per page

        public IActionResult Index(string category = "Paint", int page = 1)
        {
            List<ImageModel> images = new List<ImageModel>();

            string categoryPath = Path.Combine(_basePath, category);
            if (Directory.Exists(categoryPath))
            {
                var allFiles = Directory.GetFiles(categoryPath)
                                        .Select(file => new ImageModel
                                        {
                                            FileName = Path.GetFileName(file),
                                            FilePath = $"/Images/Gallery/{category}/{Path.GetFileName(file)}",
                                            Category = category
                                        })
                                        .ToList();

                // Apply Pagination
                images = allFiles.Skip((page - 1) * PageSize).Take(PageSize).ToList();
                ViewBag.TotalPages = (int)System.Math.Ceiling((double)allFiles.Count / PageSize);
            }

            ViewBag.Categories = GetCategories();
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentPage = page;

            return View(images);
        }

        private List<string> GetCategories()
        {
            return Directory.GetDirectories(_basePath)
                            .Select(Path.GetFileName)
                            .ToList(); // Get folder names as categories
        }
    }
}
