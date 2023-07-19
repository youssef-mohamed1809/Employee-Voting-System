using Employee_Voting_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Employee_Voting_System.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        //private readonly BankdbContext _dbContext;

        public LoginController(ILogger<LoginController> logger)
        {
            
            _logger = logger;
            //_dbContext = bankdbContext;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
       public IActionResult Index(Login login)
        {
            string loginResponse = validateLogin(login);
            
            switch(loginResponse)
            {
                case "Manager":
                    TempData["Username"] = login.username;
                    return RedirectToAction("Index", "Manager");
                case "Employee":
                    TempData["Username"] = login.username;
                    return RedirectToAction("Index", "Employee");
                case "Wrong Password":
                    ViewBag.loginStatus = loginResponse;
                    return View();
                default:
                    ViewBag.loginStatus = loginResponse;
                    return View();                    
            }
        }
        
        private string validateLogin(Login login)
        {
            List<Employee> employees;
            List<Manager> managers;
            using(var dbContext = new BankdbContext())
            {
              
                 employees = dbContext.Employee.ToList();
                 managers = dbContext.Manager.ToList();
            }


            for(int i = 0; i < employees.Count; i++)
            {
                if (employees[i].Username == login.username) {
                    if (employees[i].Password == login.password)
                    {
                        return "Employee";
                    }
                    else
                    {
                        return "Wrong Password";
                    }
                }
            }

            for(int i = 0;i < managers.Count; i++)
            {
                if (managers[i].Username == login.username)
                {
                    if (managers[i].Password == login.password)
                    {
                        return "Manager";
                    }
                    else
                    {
                        return "Wrong Password";
                    }
                }
            }

            return "Invalid Login";

            
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}