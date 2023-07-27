using backend.Models;
using backend.Models.API_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        [HttpGet("getEmployees/")]
        public async Task<ActionResult<int>> getDepartmentEmployees(int empID)
        {
            List<Employee> employees = new List<Employee>();
            using (var dbContext = new BankdbContext())
            {
                int? depID = -1;
                var list = dbContext.Employee.ToList();
                foreach (var employee in list)
                {
                    if (employee.EmpId == empID)
                    {
                        depID = employee.DepId;
                        break;
                    }
                }
                employees = dbContext.Employee.FromSql($"select * from Employee where depID = {depID}").ToList();
            }
            for (int i = 0; i < employees.Count; i++)
            {
                employees[i].Password = "";
                if (empID == employees[i].EmpId)
                {
                    employees.RemoveAt(i);
                }
            }
            return Ok(employees);
        }
        
        [HttpGet("/getEmployeeData")]
        public async Task<ActionResult<int>> getEmployeeData(int empID)
        {
            Employee employee = new Employee();
            using(var dbContext = new BankdbContext())
            {
                foreach(var e in dbContext.Employee.ToList())
                {
                    if(e.EmpId == empID)
                    {
                        e.Password = "";
                        return Ok(e);
                    }
                }
            }
            return Ok(null);
        }
    }
}
