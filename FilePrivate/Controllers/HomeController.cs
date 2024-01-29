using FilePrivate.Extensions;
using FilePrivate.Models;
using FilePrivate.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FilePrivate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileUploadService _fileUploadService;

        public HomeController(
            ILogger<HomeController> logger,
            IFileUploadService fileUploadService
            )
        {
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(UploadFileDto dto)
        {
            if (ModelState.IsValid) {
                var result = await _fileUploadService.UploadFileAsync(dto);
                if (result.IsNullOrEmpty()) {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity,"File upload operation failed");
                }
                return Ok(dto);
            }
            return BadRequest();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
