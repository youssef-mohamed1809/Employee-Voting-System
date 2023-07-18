using Employee_Voting_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Voting_System.Controllers
{
    public class ManagerController : Controller
    {
/*        private readonly BankdbContext _bankdbContext;
        public ManagerController(BankdbContext bankdbContext)
        {
            this._bankdbContext = bankdbContext;
        }*/
        public IActionResult Index()
        {
            return View();
        }
    }
}
