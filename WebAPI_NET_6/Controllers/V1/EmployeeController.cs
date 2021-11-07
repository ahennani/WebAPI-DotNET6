using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_NET_6.Controllers.V1
{
    [Route("/employees")]
    [ApiController]
    [ApiVersion("1.0")]
    public class EmployeeController : ControllerBase
    {
        private readonly IAppRepository<Employee> _employeeRepository;

        public EmployeeController(IAppRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync()
        {
            var emps = (await _employeeRepository.GetAll()).Include(emp => emp.Department);

            return Ok(emps);
        }
    }
}
