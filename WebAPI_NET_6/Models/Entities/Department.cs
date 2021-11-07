namespace WebAPI_NET_6.Models.Entities;

public class Department
{
    [Key]
    public Guid DepartmentId { get; set; }

    [Required, MaxLength(55)]
    public string Name { get; set; }

    public virtual List<Employee> Employees { get; set; }
}


