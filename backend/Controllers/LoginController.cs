using backend.Models;
using backend.Models.API_Models;
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
        public async Task<ActionResult<APIEmployee>> Login(APILogin login)
        { 
            APIEmployee e = validateLogin(login.username, login.password);

            if(e == null)
            {
                return BadRequest();
            }
            
            return Ok(e);
        }


        /*NON ACTION FUNCTIONS*/
        [NonAction]
        public APIEmployee validateLogin(string username, string password)
        {
            List<Employee> employees = new List<Employee>();
            List<Manager> managers = new List<Manager>();
            APIEmployee employee1 = new APIEmployee();
            using (var dbContext = new BankdbContext())
            {
                employees = dbContext.Employee.ToList();
                managers = dbContext.Manager.ToList();
                
            }
            foreach(var employee in employees)
            {
                if(employee.Username == username && employee.Password == password) { 
                    //APIEmployee employee1 = new APIEmployee();
                    employee1.id = employee.EmpId;
                    employee1.name = employee.Name;
                    employee1.role = "Employee";
                   return employee1;
                }
            }
            foreach (var manager in managers)
            {
                if (manager.Username == username && manager.Password == password)
                {
                    //APIEmployee employee1 = new APIEmployee();
                    employee1.id = manager.Id;
                    employee1.role = "Manager";
                    employee1.name = manager.Name;
                    return employee1;
                }
            }
            return employee1;    
        }
    }
}
