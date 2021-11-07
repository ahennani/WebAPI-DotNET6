namespace WebAPI_NET_6.Models.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Depatments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(emp =>
        {
            emp.HasKey(k => k.EmployeeID);
            emp.HasOne(e => e.Department)
               .WithMany(d => d.Employees)
               .HasForeignKey(e => e.DepartmentID);
        });

        modelBuilder.Entity<Department>(dept =>
        {
            dept.HasKey(k => k.DepartmentId);
            dept.HasMany(d => d.Employees)
                .WithOne(e => e.Department);
        });

        base.OnModelCreating(modelBuilder);
    }
}