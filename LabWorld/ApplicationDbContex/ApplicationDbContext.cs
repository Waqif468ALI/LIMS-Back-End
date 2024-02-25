using LabWorld.Model;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

   
    public DbSet<Patient> Patients { get; set; }

    public DbSet<UserModel> User_Details { get; set; }

    public DbSet<Test> Tests { get; set; }
    public DbSet<Prescriptions> Prescription { get; set; }
    public DbSet<GetPresciptions> GetPresciption { get; set; }
    public DbSet<Reports> Reports { get; set; }
    public DbSet<Dashboard> Dashboards { get; set; }
    public DbSet<LogoImage> LogoImages { get; set; }
    public DbSet<LabDetails> LabDetails { get; set; }






}
