using Crud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crud.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly HassoftRecordContext _hassoftRecordContext;

        public AccountController(ILogger<HomeController> logger, HassoftRecordContext hassoftRecordContext)
        {
            _logger = logger;
            _hassoftRecordContext = hassoftRecordContext;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult SubmitLogin(AccountUser accountUser)
        {
            if (accountUser.UserId == null || accountUser.Password == null)
            {
                return BadRequest(new { success = false , Value = "Credentials Fields"}); 
            }

            var result = _hassoftRecordContext.AccountUsers
                .FirstOrDefault(x => x.UserId == accountUser.UserId && x.Password == accountUser.Password);
            if (result != null)
            {
                return Ok(new { success = true, ErrorCode = 000 });
            }
            return View();
        }
    }
}
