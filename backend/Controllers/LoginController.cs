using backend.Models;
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
            bool loginValid = validateLogin(login.username, login.password);

            return Ok(loginValid);
        }


        /*NON ACTION FUNCTIONS*/
        [NonAction]
        public bool validateLogin(string username, string password)
        {
            List<Employee> employees = new List<Employee>();
            using(var dbContext = new BankdbContext())
            {
                employees = dbContext.Employee.ToList();
                
            }
            foreach(var employee in employees)
            {
                if(employee.Username == username && employee.Password == password) { 
                   return true;
                }
            }
            return false;
        }
    }
}
