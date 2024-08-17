using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDDemo.Controllers
{
    [Route("[controller]/[action]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesService _countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            this._countriesService = countriesService;
        }

        [HttpGet]
        [Route("upload")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Create(IFormFile excelFile)
        {
            //validation
            if(excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "No File Uploaded.";
                return View();
            }

            if(!excelFile.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") || !Path.GetExtension(excelFile.FileName).Equals(".xlsx"))
            {
                ViewBag.ErrorMessage = "Invalid file type. Please upload an Excel file.";
                return View();
            }

            MemoryStream memoryStream = new MemoryStream();
            await excelFile.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var response =  await _countriesService.AddCountryExcelFile(memoryStream);

            ViewBag.Message = response.Message;
            return View();

        }


    }
}
