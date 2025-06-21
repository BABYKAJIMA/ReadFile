using CameraView.Data;
using CameraView.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Text;

namespace CameraView.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index() => View();

        [Authorize]
        public IActionResult CameraView()
        {
            ViewBag.Username = User.Identity?.Name;
            ViewBag.Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "User";

            var deviceList = _context.DeviceRecords.ToList();
            ViewBag.DeviceRecords = deviceList;

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CameraView(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var records = new List<DeviceRecord>();

                try
                {
                    using var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
                    using var reader = ExcelReaderFactory.CreateReader(stream);

                    var rows = new List<List<object>>();

                    do
                    {
                        while (reader.Read())
                        {
                            var row = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                                row.Add(reader.GetValue(i)?.ToString() ?? "");
                            rows.Add(row);
                        }
                    } while (reader.NextResult());

                    if (rows.Count > 1)
                    {
                        for (int i = 1; i < rows.Count; i++)
                        {
                            var r = rows[i];
                            records.Add(new DeviceRecord
                            {
                                STT = r.ElementAtOrDefault(0)?.ToString(),
                                LoaiKetNoi = r.ElementAtOrDefault(1)?.ToString(),
                                TenThietBi = r.ElementAtOrDefault(2)?.ToString(),
                                IP = r.ElementAtOrDefault(3)?.ToString(),
                                ChannelZone = r.ElementAtOrDefault(4)?.ToString(),
                                VLAN = r.ElementAtOrDefault(5)?.ToString(),
                                Port = r.ElementAtOrDefault(6)?.ToString(),
                                TuRack = r.ElementAtOrDefault(7)?.ToString(),
                                ODFPatchPannel = r.ElementAtOrDefault(8)?.ToString(),
                                SoSoiPort = r.ElementAtOrDefault(9)?.ToString(),
                                TenNhanDan = r.ElementAtOrDefault(10)?.ToString(),
                                ODF1 = r.ElementAtOrDefault(11)?.ToString(),
                                SoSoi1 = r.ElementAtOrDefault(12)?.ToString(),
                                TenNhanDan2 = r.ElementAtOrDefault(13)?.ToString(),
                                ODF2 = r.ElementAtOrDefault(14)?.ToString(),
                                SoSoi2 = r.ElementAtOrDefault(15)?.ToString(),
                                DiemDauCuoi = r.ElementAtOrDefault(16)?.ToString()
                            });
                        }

                        _context.DeviceRecords.AddRange(records);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi xử lý Excel.");
                    TempData["Error"] = "Lỗi xử lý file Excel.";
                }
            }

            return RedirectToAction("CameraView");
        }

        [HttpGet]
        public async Task<IActionResult> EditDeviceAjax(int id)
        {
            var device = await _context.DeviceRecords.FindAsync(id);
            if (device == null) return Json(new { success = false });

            return Json(new { success = true, data = device });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDeviceAjax([FromBody] DeviceRecord model)
        {
            var device = await _context.DeviceRecords.FindAsync(model.Id);
            if (device == null) return Json(new { success = false });

            device.STT = model.STT;
            device.LoaiKetNoi = model.LoaiKetNoi;
            device.TenThietBi = model.TenThietBi;
            device.IP = model.IP;
            device.ChannelZone = model.ChannelZone;
            device.VLAN = model.VLAN;
            device.Port = model.Port;
            device.TuRack = model.TuRack;
            device.ODFPatchPannel = model.ODFPatchPannel;
            device.SoSoiPort = model.SoSoiPort;
            device.TenNhanDan = model.TenNhanDan;
            device.ODF1 = model.ODF1;
            device.SoSoi1 = model.SoSoi1;
            device.TenNhanDan2 = model.TenNhanDan2;
            device.ODF2 = model.ODF2;
            device.SoSoi2 = model.SoSoi2;
            device.DiemDauCuoi = model.DiemDauCuoi;

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDeviceAjax(int id)
        {
            var device = await _context.DeviceRecords.FindAsync(id);
            if (device == null) return Json(new { success = false });

            _context.DeviceRecords.Remove(device);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddDeviceAjax([FromBody] DeviceRecord device)
        {
            _context.DeviceRecords.Add(device);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult DeleteRowAjax([FromBody] int index)
        {
            var deviceList = _context.DeviceRecords.ToList();
            if (index >= 0 && index < deviceList.Count)
            {
                _context.DeviceRecords.Remove(deviceList[index]);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var data = await _context.DeviceRecords.ToListAsync();
            if (data.Count == 0) return RedirectToAction("CameraView");

            var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Kết nối mạng");

            var header = new[]
            {
                "STT", "Loại kết nối", "Tên thiết bị", "IP", "Channel/Zone", "VLAN", "Port", "Tủ Rack", "ODF Patch",
                "Sợi/Port", "Tên nhãn", "ODF 1", "Sợi 1", "Tên nhãn 2", "ODF 2", "Sợi 2", "Điểm đầu/cuối"
            };

            for (int i = 0; i < header.Length; i++)
                ws.Cells[1, i + 1].Value = header[i];

            for (int row = 0; row < data.Count; row++)
            {
                var d = data[row];
                ws.Cells[row + 2, 1].Value = d.STT;
                ws.Cells[row + 2, 2].Value = d.LoaiKetNoi;
                ws.Cells[row + 2, 3].Value = d.TenThietBi;
                ws.Cells[row + 2, 4].Value = d.IP;
                ws.Cells[row + 2, 5].Value = d.ChannelZone;
                ws.Cells[row + 2, 6].Value = d.VLAN;
                ws.Cells[row + 2, 7].Value = d.Port;
                ws.Cells[row + 2, 8].Value = d.TuRack;
                ws.Cells[row + 2, 9].Value = d.ODFPatchPannel;
                ws.Cells[row + 2, 10].Value = d.SoSoiPort;
                ws.Cells[row + 2, 11].Value = d.TenNhanDan;
                ws.Cells[row + 2, 12].Value = d.ODF1;
                ws.Cells[row + 2, 13].Value = d.SoSoi1;
                ws.Cells[row + 2, 14].Value = d.TenNhanDan2;
                ws.Cells[row + 2, 15].Value = d.ODF2;
                ws.Cells[row + 2, 16].Value = d.SoSoi2;
                ws.Cells[row + 2, 17].Value = d.DiemDauCuoi;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }
}
