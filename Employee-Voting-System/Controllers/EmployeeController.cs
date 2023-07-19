using Employee_Voting_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Employee_Voting_System.Controllers
{
    public class EmployeeController : Controller
    {
        
        private string EmployeeUsername;
        private Employee employee;
        public IActionResult Index()
        {
            employee = GetEmployeeData();
            TempData["EmployeeName"] = employee.EmpId;
            return View();
        }

        private Employee GetEmployeeData()
        {
            Employee e = new Employee();
            List<Employee> employees;
            using(var dbContext = new BankdbContext()) { 
                employees = dbContext.Employee.ToList();
            }
            foreach (Employee employee in employees)
            {
                if (employee.Username == EmployeeUsername)
                {
                    e = employee;
                }
            }
            return e;
        }
        
        private bool hasEmployeeVoted()
        {
            employee = GetEmployeeData();
            string currentYear = DateTime.Now.Year.ToString();
            List<Voting> res;
            
            using( var dbContext = new BankdbContext())
            {
                res = dbContext.Votings.FromSql($"select * from Votings where empID = {employee.EmpId} and year = {currentYear}").ToList();
            }

            if (res.IsNullOrEmpty())
            {
                return false;
            }
            return true;
        }
    
        public IActionResult Vote()
        {
            bool hasVoted = hasEmployeeVoted();
            ViewBag.hasVoted = hasVoted;
            return View();
        }
    }
}
