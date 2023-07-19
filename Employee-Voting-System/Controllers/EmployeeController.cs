using Employee_Voting_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Employee_Voting_System.Controllers
{
    public class EmployeeController : Controller
    {
        
        private string EmployeeUsername;
        private Employee employee;
        public IActionResult Index()
        {
            EmployeeUsername = (string)TempData["Username"];
            employee = GetEmployeeData();
            ViewBag.name = employee.Name;
            return View();
        }

        public IActionResult Vote()
        {
            bool hasVoted = hasEmployeeVoted();
            ViewBag.hasVoted = hasVoted;

            bool votingOpened = votingIsOpen();
            ViewBag.votingOpened = votingOpened;
            return View();
        }

        [HttpGet]
        public IActionResult getEmployees()
        {
            employee = GetEmployeeData();
            List<Employee> employees;
            using(var dbcontext = new BankdbContext())
            {
                employees = dbcontext.Employee.ToList();
            }

            for(int i = 0; i < employees.Count; i++)
            {
                if (employees[i].EmpId == employee.EmpId)
                {
                    employees.RemoveAt(i);
                }
            }
            string json = JsonSerializer.Serialize(employees);
            return Ok(json);

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

        private bool votingIsOpen()
        {
            employee = GetEmployeeData();
            DateTime currentDate = DateTime.Today;
            string date = currentDate.ToString("yyyy-MM-dd");
            List<VotingYear> res;
            using ( var dbContext = new BankdbContext())
            {
                res = dbContext.VotingYear.FromSql($"select * from VotingYear where {date} > startDate and {date} < endDate ").ToList();
            }

            if (res.IsNullOrEmpty())
            {
                return false;
            }
            return true;
        }
    

    }
}
