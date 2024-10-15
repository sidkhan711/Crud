using Crud.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Crud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HassoftRecordContext _hassoftRecordContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, HassoftRecordContext hassoftRecordContext , IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _hassoftRecordContext = hassoftRecordContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int? Id)
        {
            var result = _hassoftRecordContext.Countries.ToList();
            ViewBag.Country = JsonConvert.SerializeObject(result);
            if (Id.HasValue)
            {
                var result1 = _hassoftRecordContext.Users.Where(u => u.UserId == Id).FirstOrDefault();

                ViewBag.EditMode = JsonConvert.SerializeObject(result1);
            }
            return View();
        }
        public IActionResult SaveUser(User user , IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");

                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                var fileExtension = Path.GetExtension(ImageFile.FileName);

                var uniqueFileName = $"{fileNameWithoutExtension}_{DateTime.Now.Ticks}{fileExtension}";
                var fullPath = Path.Combine(imagePath, uniqueFileName);

                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                user.PictureProfileUrl = "/Uploads/" + uniqueFileName;
            }
            if (user.UserId == 0)
            {
                var NewId = (_hassoftRecordContext.Users.Max(u => (int?)u.UserId) ?? 0) + 1;
                user.UserId = NewId;
                _hassoftRecordContext.Users.Add(user);
                _hassoftRecordContext.SaveChanges();

                return Json(new { ErrorCode = 200 });
            }
            else
            {
                var existingUser = _hassoftRecordContext.Users.Where(u => u.UserId == user.UserId).AsNoTracking().FirstOrDefault();

                if (existingUser != null)
                {
                    if (ImageFile == null || ImageFile.Length == 0)
                    {
                        user.PictureProfileUrl = existingUser.PictureProfileUrl;
                    }
                    _hassoftRecordContext.Users.Attach(user);
                    _hassoftRecordContext.Entry(user).State = EntityState.Modified;
                }
            }
            _hassoftRecordContext.SaveChanges();
            return Json(new { ErrorCode = 200 });
        }

        public IActionResult Grid()
        {
            DataSet ds = new DataSet();

            using (SqlConnection cn = new SqlConnection(_hassoftRecordContext.Database.GetConnectionString()))
            {
                using (SqlCommand command = new SqlCommand("getData", cn))
                {
                    using (SqlDataAdapter sdapt = new SqlDataAdapter(command))
                    {
                        cn.Open();
                        sdapt.Fill(ds);
                    }
                }
                ViewBag.Record = ds.Tables[0].Rows;
            }
            
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
    }
}
