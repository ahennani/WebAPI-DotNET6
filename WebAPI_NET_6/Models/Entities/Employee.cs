using System.Text.Json.Serialization;

namespace WebAPI_NET_6.Models.Entities;

public class Employee
{
    [Key]
    public Guid EmployeeID { get; set; }

    [Required, MaxLength(55)]
    [Display(Name = "Full_Name")]
    public string FullName { get; set; }

    [MaxLength(100)]
    public string Function { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal Salary { get; set; }


    public Guid DepartmentID { get; set; }

    [JsonIgnore]
    public Department Department { get; set; }
}

