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
        public async Task<IActionResult> UploadFile([FromBody] UploadFileDto dto)
        {
            try {
                if (ModelState.IsValid) {
                    var result = await _fileUploadService.UploadFileAsync(dto);
                    //result.File = dto.File;
                    return Ok(result);
                }
                var invalidProperties = ModelState
                                        .Where(x => x.Value.Errors.Any())
                                        .Select(x => new { Property = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                                        .ToList();
                return BadRequest(invalidProperties);
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
