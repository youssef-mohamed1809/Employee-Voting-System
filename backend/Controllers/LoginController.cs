using backend.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<bool>> Login(APILogin login)
        { 
            int empID = validateLogin(login.username, login.password);

            if(empID != -1) {
                return Ok(empID);
            }
            return BadRequest();
        }


        /*NON ACTION FUNCTIONS*/
        [NonAction]
        public int validateLogin(string username, string password)
        {
            List<Employee> employees = new List<Employee>();
            using(var dbContext = new BankdbContext())
            {
                employees = dbContext.Employee.ToList();
                
            }
            foreach(var employee in employees)
            {
                if(employee.Username == username && employee.Password == password) { 
                   return employee.EmpId;
                }
            }
            return -1;
        }
    }
}
