using System.Diagnostics;
using System.Text;
using CameraView.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CameraView.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public IActionResult Index()
    {
        return View();
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

    public IActionResult CameraView()
    {
        if (TempData["ExcelData"] is string excelJson && !string.IsNullOrEmpty(excelJson))
        {
            ViewBag.ExcelData = JsonConvert.DeserializeObject<List<List<object>>>(excelJson);
        }

        if (TempData["SwitchData"] is string switchJson && !string.IsNullOrEmpty(switchJson))
        {
            ViewBag.SwitchData = JsonConvert.DeserializeObject<List<ExcelDataModels>>(switchJson);
        }

        TempData.Keep("ExcelData");
        TempData.Keep("SwitchData");

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> CameraView(IFormFile file)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        if (file != null && file.Length > 0)
        {
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            if (!Directory.Exists(uploadDirectory))
                Directory.CreateDirectory(uploadDirectory);

            var filePath = Path.Combine(uploadDirectory, file.FileName);

            // Lưu file vào thư mục
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var excelData = new List<List<object>>();

            // Đọc từng dòng thủ công, không bỏ qua dòng đầu tiên
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        var row = new List<object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var value = reader.GetValue(i)?.ToString() ?? "";
                            row.Add(value);
                        }

                        // Lưu mọi dòng có ít nhất một cột không trống
                        if (row.Any(cell => !string.IsNullOrWhiteSpace(cell?.ToString())))
                            excelData.Add(row);
                    }
                } while (reader.NextResult());
            }

            TempData["ExcelData"] = JsonConvert.SerializeObject(excelData);
        }

        return RedirectToAction("CameraView");
    }

    [HttpPost]
    public IActionResult SwitchForm(ExcelDataModels model)
    {
        if (string.IsNullOrWhiteSpace(model.SwitchName))
        {
            ModelState.AddModelError("SwitchName", "Tên Switch không được để trống.");
            return RedirectToAction("CameraView");
        }

        var switchList = new List<ExcelDataModels>();

        if (TempData["SwitchData"] is string switchJson && !string.IsNullOrEmpty(switchJson))
        {
            switchList = JsonConvert.DeserializeObject<List<ExcelDataModels>>(switchJson);
        }

        switchList.Add(model);
        TempData["SwitchData"] = JsonConvert.SerializeObject(switchList);

        TempData.Keep("ExcelData");
        TempData.Keep("SwitchData");

        return RedirectToAction("CameraView");
    }

}
