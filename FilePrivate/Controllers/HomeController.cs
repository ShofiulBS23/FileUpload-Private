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
            try {
                if (ModelState.IsValid) {
                    var result = await _fileUploadService.UploadFileAsync(dto);
                    //result.File = dto.File;
                    return Ok(result);
                }
                return BadRequest("Payload is not valid");
            }catch(Exception ex) {
                return BadRequest(ex.Message);
            }
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
