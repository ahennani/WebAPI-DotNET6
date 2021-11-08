namespace WebAPI_NET_6.Models.Data;

public class SeedData
{
    public static async Task InitializeAsync(IServiceProvider service)
    {
        var contect = service.GetRequiredService<AppDbContext>();
        await SeedAsync(contect);

        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        await AddRolesToDatabaseAsync(roleManager);
    }

    protected static async Task SeedAsync(AppDbContext dbContext)
    {
        var isChanged = false;

        if (dbContext.Depatments.Any() is false)
        {
            await dbContext.Depatments.AddRangeAsync(new List<Department>
                {
                    new Department
                    {
                        DepartmentId = new Guid("69ED71D7-EE88-43FC-9588-D53F4BF55875"),
                        Name = "Department 1"
                    },
                    new Department
                    {
                        DepartmentId = new Guid("EA5F8C7D-1212-4E89-B271-D674C9B28863"),
                        Name = "Department 2"
                    },
                    new Department
                    {
                        DepartmentId = new Guid("BD96FCCD-235E-45CB-A37C-AE86D459D90C"),
                        Name = "Department 3"
                    }
                });

            isChanged = true;
        }

        if (dbContext.Employees.Any() is false)
        {
            await dbContext.Employees.AddRangeAsync(new List<Employee>
                {
                    new Employee
                    {
                        EmployeeID = new Guid("68863746-F139-485C-BB66-F45AAB1CAEBB"),
                        FullName = "Employee 01",
                        Function = "Function 01",
                        Salary = 5600,
                        DepartmentID = new Guid("69ED71D7-EE88-43FC-9588-D53F4BF55875")
                    },
                    new Employee
                    {
                        EmployeeID = new Guid("77A19FC0-5011-4427-ACB8-7F50B5F14254"),
                        FullName = "Employee 02",
                        Function = "Function 02",
                        Salary = 3000,
                        DepartmentID = new Guid("69ED71D7-EE88-43FC-9588-D53F4BF55875")
                    },
                    new Employee
                    {
                        EmployeeID = new Guid("2880AC97-A513-484C-BE7B-045B032CB46E"),
                        FullName = "Employee 03",
                        Function = "Function 03",
                        Salary = 6000,
                        DepartmentID = new Guid("EA5F8C7D-1212-4E89-B271-D674C9B28863")
                    },
                    new Employee
                    {
                        EmployeeID = new Guid("DD28AC1C-4423-4C79-89CA-482EFBF82552"),
                        FullName = "Employee 04",
                        Function = "Function 04",
                        Salary = 4550,
                        DepartmentID = new Guid("EA5F8C7D-1212-4E89-B271-D674C9B28863")
                    },
                    new Employee
                    {
                        EmployeeID = new Guid("EB5D005E-607E-4BBB-8C06-A411C96F45B4"),
                        FullName = "Employee 05",
                        Function = "Function 05",
                        Salary = 2500,
                        DepartmentID = new Guid("EA5F8C7D-1212-4E89-B271-D674C9B28863")
                    }
                });

            isChanged = true;
        }


        if (isChanged is true)
            await dbContext.SaveChangesAsync();
    }

    protected static async Task AddRolesToDatabaseAsync(RoleManager<IdentityRole> roleManager)
    {
        var rolesCount = roleManager.Roles.Count();
        var enumRoles = Enum.GetValues(typeof(Roles));

        if (rolesCount >= enumRoles.Length)
            return;

        foreach (var item in enumRoles)
        {
            var role = item.ToString();

            var result = await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
