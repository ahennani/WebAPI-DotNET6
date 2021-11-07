namespace WebAPI_NET_6.Models.Repositories
{
    public class EmployeeRepository : IAppRepository<Employee>
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context) => _context = context;

        public Task<IQueryable<Employee>> GetAll() => Task.Run(()=> _context.Employees.AsNoTracking());
    }
}
